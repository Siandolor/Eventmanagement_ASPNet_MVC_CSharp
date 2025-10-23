namespace Eventmanagement.Controllers;

[Authorize(Roles = nameof(UserRole.Admin) + "," +
                   nameof(UserRole.Coworker) + "," +
                   nameof(UserRole.Customer) + "," +
                   nameof(UserRole.Location) + "," +
                   nameof(UserRole.Moderator) + "," +
                   nameof(UserRole.Organizer) + "," +
                   nameof(UserRole.Performer))]
[Authorize(Policy = "RegisteredOnly")]
public class UserProfilesController : Controller
{
    private readonly EventmanagementContext _context;
    private readonly IWebHostEnvironment _env;

    public UserProfilesController(EventmanagementContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Profile()
    {
        var loginId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var login = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Location)
            .Include(l => l.Moderator)
            .Include(l => l.Organizer)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == loginId);

        if (login == null)
            return RedirectToAction("Index", "Home");

        UserRole role = UserRole.Unknown;
        if (login.Customer != null) role = login.Customer.Customer_Role;
        else if (login.InternalCoworker != null) role = login.InternalCoworker.InternalCoworker_Role;
        else if (login.Location != null) role = login.Location.Location_Role;
        else if (login.Moderator != null) role = login.Moderator.Moderator_Role;
        else if (login.Organizer != null) role = login.Organizer.Organizer_Role;
        else if (login.Performer != null) role = login.Performer.Performer_Role;

        ViewData["UserRole"] = role;

        if (login.Organizer != null)
        {
            var organizerId = login.Organizer.Organizer_Id;

            var events = await _context.EventBasics
                .Where(e => e.Organizer_Id == organizerId)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .ToListAsync();

            var sessions = await _context.EventSessions
                .Where(s => events.Select(e => e.Event_Id).Contains(s.Event_Id))
                .ToListAsync();

            var sessionPresenceMap = sessions
                .GroupBy(s => s.Event_Id)
                .ToDictionary(g => g.Key, g => g.Any());

            var sessionMap = sessions
                .GroupBy(s => s.Event_Id)
                .ToDictionary(g => g.Key, g => g.First());

            var attachments = await _context.EventAttachments
                .Where(a => events.Select(e => e.Event_Id).Contains(a.Event_Id))
                .ToListAsync();

            var attachmentMap = attachments
                .GroupBy(a => a.Event_Id)
                .ToDictionary(g => g.Key, g => g.Any());

            ViewBag.OrganizerEvents = events;
            ViewBag.HasSession = sessionPresenceMap;
            ViewBag.SessionMap = sessionMap;
            ViewBag.HasAttachment = attachmentMap;
        }

        return View("Profile", login);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserBio(Guid id, string bio)
    {
        var login = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Location)
            .Include(l => l.Moderator)
            .Include(l => l.Organizer)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login.Customer != null) login.Customer.Customer_ProfileBio = bio;
        else if (login.InternalCoworker != null) login.InternalCoworker.InternalCoworker_ProfileBio = bio;
        else if (login.Location != null) login.Location.Location_ProfileBio = bio;
        else if (login.Moderator != null) login.Moderator.Moderator_ProfileBio = bio;
        else if (login.Organizer != null) login.Organizer.Organizer_ProfileBio = bio;
        else if (login.Performer != null) login.Performer.Performer_ProfileBio = bio;

        await _context.SaveChangesAsync();
        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UploadUserImage(Guid id, IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Nur JPG, PNG, GIF oder WEBP-Dateien sind erlaubt.");
                return RedirectToAction("Profile");
            }

            using var image = await SixLabors.ImageSharp.Image.LoadAsync(imageFile.OpenReadStream());

            int size = Math.Min(image.Width, image.Height);
            int x = (image.Width - size) / 2;
            int y = (image.Height - size) / 2;
            image.Mutate(i => i.Crop(new Rectangle(x, y, size, size)));

            var fileName = $"{id}_profile.png";
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "profile");
            Directory.CreateDirectory(uploadPath);
            var filePath = Path.Combine(uploadPath, fileName);

            await image.SaveAsync(filePath, new PngEncoder());

            var login = await _context.LogIns
                .Include(l => l.Customer)
                .Include(l => l.InternalCoworker)
                .Include(l => l.Location)
                .Include(l => l.Moderator)
                .Include(l => l.Organizer)
                .Include(l => l.Performer)
                .FirstOrDefaultAsync(l => l.LogIn_Id == id);

            var imagePath = $"/uploads/profile/{fileName}";

            if (login.Customer != null) login.Customer.Customer_ProfileImage = imagePath;
            else if (login.InternalCoworker != null) login.InternalCoworker.InternalCoworker_ProfileImage = imagePath;
            else if (login.Location != null) login.Location.Location_ProfileImage = imagePath;
            else if (login.Moderator != null) login.Moderator.Moderator_ProfileImage = imagePath;
            else if (login.Organizer != null) login.Organizer.Organizer_ProfileImage = imagePath;
            else if (login.Performer != null) login.Performer.Performer_ProfileImage = imagePath;

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserAdress(Guid id, string streetName, string streetNumber, string apartmentNumber, string zip, string city, string state, string country)
    {
        var login = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Location)
            .Include(l => l.Moderator)
            .Include(l => l.Organizer)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Customer != null)
        {
            var user = login.Customer;
            user.Customer_StreetName = streetName;
            user.Customer_StreetNumber = streetNumber;
            user.Customer_ApartmentNumber = apartmentNumber;
            user.Customer_ZIP = zip;
            user.Customer_City = city;
            user.Customer_State = state;
            user.Customer_Country = country;

            await _context.SaveChangesAsync();
        }
        else if (login?.InternalCoworker != null)
        {
            var user = login.InternalCoworker;
            user.InternalCoworker_StreetName = streetName;
            user.InternalCoworker_StreetNumber = streetNumber;
            user.InternalCoworker_ApartmentNumber = apartmentNumber;
            user.InternalCoworker_ZIP = zip;
            user.InternalCoworker_City = city;
            user.InternalCoworker_State = state;
            user.InternalCoworker_Country = country;

            await _context.SaveChangesAsync();
        }
        else if (login?.Location != null)
        {
            var user = login.Location;
            user.Location_StreetName = streetName;
            user.Location_StreetNumber = streetNumber;
            user.Location_ApartmentNumber = apartmentNumber;
            user.Location_ZIP = zip;
            user.Location_City = city;
            user.Location_State = state;
            user.Location_Country = country;

            await _context.SaveChangesAsync();
        }
        else if (login?.Organizer != null)
        {
            var user = login.Organizer;
            user.Organizer_StreetName = streetName;
            user.Organizer_StreetNumber = streetNumber;
            user.Organizer_ApartmentNumber = apartmentNumber;
            user.Organizer_ZIP = zip;
            user.Organizer_City = city;
            user.Organizer_State = state;
            user.Organizer_Country = country;

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserPhone(Guid id, string phone)
    {
        var login = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Location)
            .Include(l => l.Moderator)
            .Include(l => l.Organizer)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Customer != null)
        {
            login.Customer.Customer_Phone = phone;
            await _context.SaveChangesAsync();
        }
        if (login?.InternalCoworker != null)
        {
            login.InternalCoworker.InternalCoworker_Phone = phone;
            await _context.SaveChangesAsync();
        }
        if (login?.Location != null)
        {
            login.Location.Location_Phone = phone;
            await _context.SaveChangesAsync();
        }
        if (login?.Moderator != null)
        {
            login.Moderator.Moderator_Phone = phone;
            await _context.SaveChangesAsync();
        }
        if (login?.Organizer != null)
        {
            login.Organizer.Organizer_Phone = phone;
            await _context.SaveChangesAsync();
        }
        if (login?.Performer != null)
        {
            login.Performer.Performer_Phone = phone;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateWebsite(Guid id, string website)
    {
        var login = await _context.LogIns
            .Include(l => l.Location)
            .Include(l => l.Organizer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Location != null)
        {
            login.Location.Location_WebSiteURL = website;
            await _context.SaveChangesAsync();
        }
        if (login?.Organizer != null)
        {
            login.Organizer.Organizer_WebSiteURL = website;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateContactPerson(Guid id, string contactPerson)
    {
        var login = await _context.LogIns
            .Include(l => l.Location)
            .Include(l => l.Organizer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Location != null)
        {
            login.Location.Location_ContactPerson = contactPerson;
            await _context.SaveChangesAsync();
        }
        if (login?.Organizer != null)
        {
            login.Organizer.Organizer_ContactPerson = contactPerson;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateManagement(Guid id, string management)
    {
        var login = await _context.LogIns
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Performer != null)
        {
            login.Performer.Performer_Management = management;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSeatings(Guid id, int seatings)
    {
        var login = await _context.LogIns
            .Include(l => l.Location)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Location != null)
        {
            login.Location.Location_SeatingsAmount = seatings;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStandings(Guid id, int standings)
    {
        var login = await _context.LogIns
            .Include(l => l.Location)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Location != null)
        {
            login.Location.Location_StandingsAmount = standings;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserEmail(Guid id, string email)
    {
        var login = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Location)
            .Include(l => l.Moderator)
            .Include(l => l.Organizer)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login != null)
        {
            login.LogIn_Name = email;

            if (login.Customer != null) login.Customer.Customer_Email = email;
            else if (login.InternalCoworker != null) login.InternalCoworker.InternalCoworker_Email = email;
            else if (login.Location != null) login.Location.Location_Email = email;
            else if (login.Moderator != null) login.Moderator.Moderator_Email = email;
            else if (login.Organizer != null) login.Organizer.Organizer_Email = email;
            else if (login.Performer != null) login.Performer.Performer_Email = email;

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserPassword(Guid id, string newPassword)
    {
        var login = await _context.LogIns.FindAsync(id);
        if (login != null)
        {
            login.LogIn_Password = GenerateHash.HashPassword(newPassword);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }

    [HttpGet]
    public async Task<IActionResult> OrganizerEvents()
    {
        var loginId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var login = await _context.LogIns
            .Include(l => l.Organizer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == loginId);

        if (login?.Organizer == null)
            return NotFound();

        var organizerId = login.Organizer.Organizer_Id;

        var events = await _context.EventBasics
            .Where(e => e.Organizer_Id == organizerId)
            .Include(e => e.Location)
            .Include(e => e.Organizer)
            .ToListAsync();

        var sessions = await _context.EventSessions
            .Include(s => s.Event)
            .ToListAsync();

        var sessionPresenceMap = sessions
            .GroupBy(s => s.Event_Id)
            .ToDictionary(g => g.Key, g => g.Any());

        var sessionMap = sessions
            .GroupBy(s => s.Event_Id)
            .ToDictionary(g => g.Key, g => g.First());

        var attachments = await _context.EventAttachments.ToListAsync();

        var attachmentMap = attachments
            .GroupBy(a => a.Event_Id)
            .ToDictionary(g => g.Key, g => g.Any());

        ViewBag.HasSession = sessionPresenceMap;
        ViewBag.SessionMap = sessionMap;
        ViewBag.HasAttachment = attachmentMap;
        ViewBag.Organizer = login.Organizer;

        return PartialView("~/Views/UserProfiles/_OrganizerEvents.cshtml", events);
    }

    public async Task<IActionResult> EventDetails(Guid? id)
    {
        if (id == null)
            return NotFound();

        var eventItem = await _context.EventBasics
            .Include(e => e.Location)
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Event_Id == id);

        if (eventItem == null)
            return NotFound();

        var seatUnits = await _context.EventSeatUnits
            .Where(s => s.Event_Id == eventItem.Event_Id)
            .Include(s => s.SeatUnit)
            .ToListAsync();

        var sessions = await _context.EventSessions
            .Where(s => s.Event_Id == eventItem.Event_Id)
            .ToListAsync();

        var attachments = await _context.EventAttachments
            .Where(a => a.Event_Id == eventItem.Event_Id)
            .ToListAsync();

        ViewBag.SeatUnits = seatUnits;
        ViewBag.Sessions = sessions;
        ViewBag.Attachments = attachments;

        return View(eventItem);
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var login = await _context.LogIns.FindAsync(id);
        if (login != null)
        {
            _context.LogIns.Remove(login);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Home");
    }
}
