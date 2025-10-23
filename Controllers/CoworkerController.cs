namespace Eventmanagement.Controllers;

[Authorize(Roles = nameof(UserRole.Coworker))]
[Authorize(Policy = "CoworkerOnly")]
public class CoworkerController : Controller
{
    private readonly EventmanagementContext _context;
    private readonly IWebHostEnvironment _env;

    public CoworkerController(EventmanagementContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public IActionResult CreateUserCoworker(string model)
    {
        return View("CreateUserCoworker", new UserRegistrationAndRoles
        {
            SelectedRole = model,
            Customer = new Customer { Customer_LogIn = new LogIn() },
            Organizer = new Organizer { Organizer_LogIn = new LogIn() },
            Location = new Location { Location_LogIn = new LogIn() },
            Admin = new InternalCoworker { InternalCoworker_LogIn = new LogIn() },
            Coworker = new InternalCoworker { InternalCoworker_LogIn = new LogIn() },
            SharedEmail = "",
            ConfirmPassword = ""
        });
    }

    [HttpPost]
    public IActionResult RegisterCustomer(UserRegistrationAndRoles model)
    {
        //ModelState.Remove("Customer");
        ModelStateCleaner.CleanCustomer(ModelState);
        ModelState.Remove("Admin");
        ModelStateCleaner.CleanAdmin(ModelState);
        ModelState.Remove("Coworker");
        ModelStateCleaner.CleanCoworker(ModelState);
        ModelState.Remove("Location");
        ModelStateCleaner.CleanLocation(ModelState);
        ModelState.Remove("Moderator");
        ModelStateCleaner.CleanModerator(ModelState);
        ModelState.Remove("Organizer");
        ModelStateCleaner.CleanOrganizer(ModelState);
        ModelState.Remove("Performer");
        ModelStateCleaner.CleanPerformer(ModelState);

        if (model.Customer?.Customer_LogIn == null)
        {
            ModelState.AddModelError("", "Modellbindung fehlgeschlagen.");
            return View("CreateUserCoworker", model);
        }

        if (model.Customer.Customer_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        model.Customer.Customer_Email = model.SharedEmail;
        model.Customer.Customer_LogIn.LogIn_Name = model.SharedEmail;

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Customer";
            return View("CreateUserCoworker", model);
        }

        var id = Guid.NewGuid();
        model.Customer.Customer_Id = id;
        model.Customer.Customer_RegistrationDate = DateTime.UtcNow;
        model.Customer.Customer_Role = UserRole.Customer;

        model.Customer.Customer_LogIn.LogIn_Id = id;
        model.Customer.Customer_LogIn.LogIn_Name = model.SharedEmail;

        model.Customer.Customer_LogIn.LogIn_Password =
            GenerateHash.HashPassword(model.Customer.Customer_LogIn.LogIn_Password);

        _context.LogIns.Add(model.Customer.Customer_LogIn);
        _context.Customers.Add(model.Customer);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult RegisterOrganizer(UserRegistrationAndRoles model)
    {
        ModelState.Remove("Customer");
        ModelStateCleaner.CleanCustomer(ModelState);
        ModelState.Remove("Admin");
        ModelStateCleaner.CleanAdmin(ModelState);
        ModelState.Remove("Coworker");
        ModelStateCleaner.CleanCoworker(ModelState);
        ModelState.Remove("Location");
        ModelStateCleaner.CleanLocation(ModelState);
        ModelState.Remove("Moderator");
        ModelStateCleaner.CleanModerator(ModelState);
        //ModelState.Remove("Organizer");
        ModelStateCleaner.CleanOrganizer(ModelState);
        ModelState.Remove("Performer");
        ModelStateCleaner.CleanPerformer(ModelState);

        if (model.Organizer?.Organizer_LogIn == null)
        {
            ModelState.AddModelError("", "Modellbindung fehlgeschlagen.");
            model.SelectedRole = "Organizer";
            return View("CreateUserCoworker", model);
        }

        if (model.Organizer.Organizer_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Organizer";
            return View("CreateUserCoworker", model);
        }

        var id = Guid.NewGuid();
        model.Organizer.Organizer_Id = id;
        model.Organizer.Organizer_Email = model.SharedEmail;
        model.Organizer.Organizer_RegistrationDate = DateTime.UtcNow;
        model.Organizer.Organizer_Role = UserRole.Organizer;

        model.Organizer.Organizer_LogIn.LogIn_Id = id;
        model.Organizer.Organizer_LogIn.LogIn_Name = model.SharedEmail;

        model.Organizer.Organizer_LogIn.LogIn_Password =
            GenerateHash.HashPassword(model.Organizer.Organizer_LogIn.LogIn_Password);

        _context.LogIns.Add(model.Organizer.Organizer_LogIn);
        _context.Organizers.Add(model.Organizer);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult RegisterLocation(UserRegistrationAndRoles model)
    {
        ModelState.Remove("Customer");
        ModelStateCleaner.CleanCustomer(ModelState);
        ModelState.Remove("Admin");
        ModelStateCleaner.CleanAdmin(ModelState);
        ModelState.Remove("Coworker");
        ModelStateCleaner.CleanCoworker(ModelState);
        //ModelState.Remove("Location");
        ModelStateCleaner.CleanLocation(ModelState);
        ModelState.Remove("Moderator");
        ModelStateCleaner.CleanModerator(ModelState);
        ModelState.Remove("Organizer");
        ModelStateCleaner.CleanOrganizer(ModelState);
        ModelState.Remove("Performer");
        ModelStateCleaner.CleanPerformer(ModelState);

        if (model.Location?.Location_LogIn == null)
        {
            ModelState.AddModelError("", "Modellbindung fehlgeschlagen.");
            model.SelectedRole = "Location";
            return View("CreateUserCoworker", model);
        }

        if (model.Location.Location_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Location";
            return View("CreateUserCoworker", model);
        }

        var id = Guid.NewGuid();
        model.Location.Location_Id = id;
        model.Location.Location_Email = model.SharedEmail;
        model.Location.Location_RegistrationDate = DateTime.UtcNow;
        model.Location.Location_Role = UserRole.Location;

        model.Location.Location_LogIn.LogIn_Id = id;
        model.Location.Location_LogIn.LogIn_Name = model.SharedEmail;

        model.Location.Location_LogIn.LogIn_Password =
            GenerateHash.HashPassword(model.Location.Location_LogIn.LogIn_Password);

        _context.LogIns.Add(model.Location.Location_LogIn);
        _context.Locations.Add(model.Location);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult RegisterAdmin(UserRegistrationAndRoles model)
    {
        ModelState.Remove("Customer");
        ModelStateCleaner.CleanCustomer(ModelState);
        //ModelState.Remove("Admin");
        ModelStateCleaner.CleanAdmin(ModelState);
        ModelState.Remove("Coworker");
        ModelStateCleaner.CleanCoworker(ModelState);
        ModelState.Remove("Location");
        ModelStateCleaner.CleanLocation(ModelState);
        ModelState.Remove("Moderator");
        ModelStateCleaner.CleanModerator(ModelState);
        ModelState.Remove("Organizer");
        ModelStateCleaner.CleanOrganizer(ModelState);
        ModelState.Remove("Performer");
        ModelStateCleaner.CleanPerformer(ModelState);

        if (model.Admin?.InternalCoworker_LogIn == null)
        {
            ModelState.AddModelError("", "Modellbindung fehlgeschlagen.");
            model.SelectedRole = "Admin";
            return View("CreateUserCoworker", model);
        }

        if (model.Admin.InternalCoworker_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Admin";
            return View("CreateUserCoworker", model);
        }

        var id = Guid.NewGuid();
        model.Admin.InternalCoworker_Id = id;
        model.Admin.InternalCoworker_Email = model.SharedEmail;
        model.Admin.InternalCoworker_RegistrationDate = DateTime.UtcNow;
        model.Admin.InternalCoworker_Role = UserRole.Admin;

        model.Admin.InternalCoworker_LogIn.LogIn_Id = id;
        model.Admin.InternalCoworker_LogIn.LogIn_Name = model.SharedEmail;

        model.Admin.InternalCoworker_LogIn.LogIn_Password =
            GenerateHash.HashPassword(model.Admin.InternalCoworker_LogIn.LogIn_Password);

        _context.LogIns.Add(model.Admin.InternalCoworker_LogIn);
        _context.InternalCoworkers.Add(model.Admin);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult RegisterCoworker(UserRegistrationAndRoles model)
    {
        ModelState.Remove("Customer");
        ModelStateCleaner.CleanCustomer(ModelState);
        ModelState.Remove("Admin");
        ModelStateCleaner.CleanAdmin(ModelState);
        //ModelState.Remove("Coworker");
        ModelStateCleaner.CleanCoworker(ModelState);
        ModelState.Remove("Location");
        ModelStateCleaner.CleanLocation(ModelState);
        ModelState.Remove("Moderator");
        ModelStateCleaner.CleanModerator(ModelState);
        ModelState.Remove("Organizer");
        ModelStateCleaner.CleanOrganizer(ModelState);
        ModelState.Remove("Performer");
        ModelStateCleaner.CleanPerformer(ModelState);

        if (model.Coworker?.InternalCoworker_LogIn == null)
        {
            ModelState.AddModelError("", "Modellbindung fehlgeschlagen.");
            model.SelectedRole = "Coworker";
            return View("CreateUserCoworker", model);
        }

        if (model.Coworker.InternalCoworker_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Coworker";
            return View("CreateUserCoworker", model);
        }

        var id = Guid.NewGuid();
        model.Coworker.InternalCoworker_Id = id;
        model.Coworker.InternalCoworker_Email = model.SharedEmail;
        model.Coworker.InternalCoworker_RegistrationDate = DateTime.UtcNow;
        model.Coworker.InternalCoworker_Role = UserRole.Coworker;

        model.Coworker.InternalCoworker_LogIn.LogIn_Id = id;
        model.Coworker.InternalCoworker_LogIn.LogIn_Name = model.SharedEmail;

        model.Coworker.InternalCoworker_LogIn.LogIn_Password =
            GenerateHash.HashPassword(model.Coworker.InternalCoworker_LogIn.LogIn_Password);

        _context.LogIns.Add(model.Coworker.InternalCoworker_LogIn);
        _context.InternalCoworkers.Add(model.Coworker);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult RegisterModerator(UserRegistrationAndRoles model)
    {
        ModelState.Remove("Customer");
        ModelStateCleaner.CleanCustomer(ModelState);
        ModelState.Remove("Admin");
        ModelStateCleaner.CleanAdmin(ModelState);
        ModelState.Remove("Coworker");
        ModelStateCleaner.CleanCoworker(ModelState);
        ModelState.Remove("Location");
        ModelStateCleaner.CleanLocation(ModelState);
        //ModelState.Remove("Moderator");
        ModelStateCleaner.CleanModerator(ModelState);
        ModelState.Remove("Organizer");
        ModelStateCleaner.CleanOrganizer(ModelState);
        ModelState.Remove("Performer");
        ModelStateCleaner.CleanPerformer(ModelState);

        if (model.Moderator?.Moderator_LogIn == null)
        {
            ModelState.AddModelError("", "Modellbindung fehlgeschlagen.");
            model.SelectedRole = "Moderator";
            return View("CreateUserCoworker", model);
        }

        if (model.Moderator.Moderator_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Moderator";
            return View("CreateUserCoworker", model);
        }

        var id = Guid.NewGuid();
        model.Moderator.Moderator_Id = id;
        model.Moderator.Moderator_Email = model.SharedEmail;
        model.Moderator.Moderator_RegistrationDate = DateTime.UtcNow;
        model.Moderator.Moderator_Role = UserRole.Moderator;

        model.Moderator.Moderator_LogIn.LogIn_Id = id;
        model.Moderator.Moderator_LogIn.LogIn_Name = model.SharedEmail;

        model.Moderator.Moderator_LogIn.LogIn_Password =
            GenerateHash.HashPassword(model.Moderator.Moderator_LogIn.LogIn_Password);

        _context.LogIns.Add(model.Moderator.Moderator_LogIn);
        _context.Moderators.Add(model.Moderator);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult RegisterPerformer(UserRegistrationAndRoles model)
    {
        ModelState.Remove("Customer");
        ModelStateCleaner.CleanCustomer(ModelState);
        ModelState.Remove("Admin");
        ModelStateCleaner.CleanAdmin(ModelState);
        ModelState.Remove("Coworker");
        ModelStateCleaner.CleanCoworker(ModelState);
        ModelState.Remove("Location");
        ModelStateCleaner.CleanLocation(ModelState);
        ModelState.Remove("Moderator");
        ModelStateCleaner.CleanModerator(ModelState);
        ModelState.Remove("Organizer");
        ModelStateCleaner.CleanOrganizer(ModelState);
        //ModelState.Remove("Performer");
        ModelStateCleaner.CleanPerformer(ModelState);

        if (model.Performer?.Performer_LogIn == null)
        {
            ModelState.AddModelError("", "Modellbindung fehlgeschlagen.");
            model.SelectedRole = "Performer";
            return View("CreateUserCoworker", model);
        }

        if (model.Performer.Performer_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Performer";
            return View("CreateUserCoworker", model);
        }

        var id = Guid.NewGuid();
        model.Performer.Performer_Id = id;
        model.Performer.Performer_Email = model.SharedEmail;
        model.Performer.Performer_RegistrationDate = DateTime.UtcNow;
        model.Performer.Performer_Role = UserRole.Performer;

        model.Performer.Performer_LogIn.LogIn_Id = id;
        model.Performer.Performer_LogIn.LogIn_Name = model.SharedEmail;

        model.Performer.Performer_LogIn.LogIn_Password =
            GenerateHash.HashPassword(model.Performer.Performer_LogIn.LogIn_Password);

        _context.LogIns.Add(model.Performer.Performer_LogIn);
        _context.Performers.Add(model.Performer);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> UserIndexCoworker(string userType)
    {
        ViewBag.UserType = userType;

        var users = userType switch
        {
            "Customer" => await _context.Customers.ToListAsync<object>(),
            "InternalCoworker" => await _context.InternalCoworkers.ToListAsync<object>(),
            "Location" => await _context.Locations.ToListAsync<object>(),
            "Moderator" => await _context.Moderators.ToListAsync<object>(),
            "Organizer" => await _context.Organizers.ToListAsync<object>(),
            "Performer" => await _context.Performers.ToListAsync<object>(),
            _ => new List<object>()
        };

        return View(users);
    }

    public async Task<IActionResult> UserDetailsCoworker(string userType, Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var login = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Location)
            .Include(l => l.Moderator)
            .Include(l => l.Organizer)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login == null) return NotFound();

        ViewData["UserRole"] = userType switch
        {
            "Customer" => login.Customer?.Customer_Role,
            "InternalCoworker" => login.InternalCoworker?.InternalCoworker_Role,
            "Admin" => login.InternalCoworker?.InternalCoworker_Role,
            "Coworker" => login.InternalCoworker?.InternalCoworker_Role,
            "Location" => login.Location?.Location_Role,
            "Moderator" => login.Moderator?.Moderator_Role,
            "Organizer" => login.Organizer?.Organizer_Role,
            "Performer" => login.Performer?.Performer_Role,
            _ => UserRole.Unknown
        };

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

        return View(login);
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

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
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

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserBirthday(Guid id, DateOnly birthday)
    {
        var login = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Moderator)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Customer != null)
        {
            login.Customer.Customer_DateOfBirth = birthday;
            await _context.SaveChangesAsync();
        }
        if (login?.InternalCoworker != null)
        {
            login.InternalCoworker.InternalCoworker_DateOfBirth = birthday;
            await _context.SaveChangesAsync();
        }
        if (login?.Moderator != null)
        {
            login.Moderator.Moderator_DateOfBirth = birthday;
            await _context.SaveChangesAsync();
        }
        if (login?.Performer != null)
        {
            login.Performer.Performer_DateOfBirth = birthday;
            await _context.SaveChangesAsync();
        }

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserContactPerson(Guid id, string contactPerson)
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

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
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

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserFIN(Guid id, string fin)
    {
        var login = await _context.LogIns
            .Include(l => l.Location)
            .Include(l => l.Organizer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Location != null)
        {
            login.Location.Location_FIN = fin;
            await _context.SaveChangesAsync();
        }
        if (login?.Organizer != null)
        {
            login.Organizer.Organizer_FIN = fin;
            await _context.SaveChangesAsync();
        }

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });

    }
    [HttpPost]
    public async Task<IActionResult> UpdateUserImage(Guid id, IFormFile imageFile)
    {
        LogIn login = null;

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

            login = await _context.LogIns
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

        var userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserManagement(Guid id, string management)
    {
        var login = await _context.LogIns
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.Performer != null)
        {
            login.Performer.Performer_Management = management;
            await _context.SaveChangesAsync();
        }

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserName(Guid id, string name, string firstName, string lastName)
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
            login.Customer.Customer_FirstName = firstName;
            login.Customer.Customer_LastName = lastName;
            await _context.SaveChangesAsync();
        }
        if (login?.InternalCoworker != null)
        {
            login.InternalCoworker.InternalCoworker_FirstName = firstName;
            login.InternalCoworker.InternalCoworker_LastName = lastName;
            await _context.SaveChangesAsync();
        }
        if (login?.Location != null)
        {
            login.Location.Location_CompanyName = name;
            await _context.SaveChangesAsync();
        }
        if (login?.Moderator != null)
        {
            login.Moderator.Moderator_FirstName = firstName;
            login.Moderator.Moderator_LastName = lastName;
            await _context.SaveChangesAsync();
        }
        if (login?.Organizer != null)
        {
            login.Organizer.Organizer_CompanyName = name;
            await _context.SaveChangesAsync();
        }
        if (login?.Performer != null)
        {
            login.Performer.Performer_Name = name;
            await _context.SaveChangesAsync();
        }

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
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

        string userType = GetUserTypeFromLogin(login);
        return RedirectToAction("UserDetailsCoworker", new { userType, id });
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

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserSalary(Guid id, decimal salary)
    {
        var login = await _context.LogIns
            .Include(l => l.InternalCoworker)
            .Include(l => l.Moderator)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(l => l.LogIn_Id == id);

        if (login?.InternalCoworker != null)
        {
            login.InternalCoworker.InternalCoworker_MonthlySalary = salary;
            await _context.SaveChangesAsync();
        }
        if (login?.Moderator != null)
        {
            login.Moderator.Moderator_EventSalary = salary;
            await _context.SaveChangesAsync();
        }
        if (login?.Performer != null)
        {
            login.Performer.Performer_EventSalary = salary;
            await _context.SaveChangesAsync();
        }

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserWebsite(Guid id, string website)
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

        string userType = GetUserTypeFromLogin(login);

        return RedirectToAction("UserDetailsCoworker", new { userType, id });
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(Guid id)
    {
        var eventItem = await _context.EventBasics
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Event_Id == id);

        if (eventItem == null || eventItem.Organizer == null) return NotFound();

        eventItem.IsPublished = !eventItem.IsPublished;
        _context.Update(eventItem);
        await _context.SaveChangesAsync();

        var loginId = await _context.LogIns
            .Where(l => l.Organizer != null && l.Organizer.Organizer_Id == eventItem.Organizer.Organizer_Id)
            .Select(l => l.LogIn_Id)
            .FirstOrDefaultAsync();

        return RedirectToAction(nameof(UserDetailsCoworker), new { userType = "Organizer", id = loginId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleTicketSale(Guid id)
    {
        var eventItem = await _context.EventBasics
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Event_Id == id);

        if (eventItem == null || eventItem.Organizer == null) return NotFound();

        eventItem.TicketSaleOpen = !eventItem.TicketSaleOpen;
        _context.Update(eventItem);
        await _context.SaveChangesAsync();

        var loginId = await _context.LogIns
            .Where(l => l.Organizer != null && l.Organizer.Organizer_Id == eventItem.Organizer.Organizer_Id)
            .Select(l => l.LogIn_Id)
            .FirstOrDefaultAsync();

        return RedirectToAction(nameof(UserDetailsCoworker), new { userType = "Organizer", id = loginId });
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

        return PartialView("~/Views/Coworker/_OrganizerEvents.cshtml", events);
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

        var loginId = await _context.LogIns
            .Where(l => l.Organizer != null && l.Organizer.Organizer_Id == eventItem.Organizer.Organizer_Id)
            .Select(l => l.LogIn_Id)
            .FirstOrDefaultAsync();

        ViewBag.OrganizerLoginId = loginId;
        ViewBag.UserType = "Organizer";

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

    [HttpGet]
    public async Task<IActionResult> EventDesigner(Guid eventId)
    {
        var organizerId = await _context.EventBasics
            .Where(e => e.Event_Id == eventId)
            .Select(e => e.Organizer_Id)
            .FirstOrDefaultAsync();

        if (organizerId == Guid.Empty)
            return NotFound();

        return RedirectToAction("Index", "EventDesigner", new { organizerId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var eventBasics = await _context.EventBasics
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Event_Id == id);

        if (eventBasics == null)
            return NotFound();

        var organizerId = eventBasics.Organizer?.Organizer_Id;

        if (organizerId == null)
            return NotFound();

        var loginId = await _context.LogIns
            .Where(l => l.Organizer != null && l.Organizer.Organizer_Id == organizerId)
            .Select(l => l.LogIn_Id)
            .FirstOrDefaultAsync();

        _context.EventBasics.Remove(eventBasics);
        await _context.SaveChangesAsync();

        return RedirectToAction("UserDetailsCoworker", new { userType = "Organizer", id = loginId });
    }

    private async Task<object> FindEntityByType(string userType, Guid id)
    {
        return userType switch
        {
            "Customer" => await _context.Customers.FindAsync(id),
            "InternalCoworker" => await _context.InternalCoworkers.FindAsync(id),
            "Location" => await _context.Locations.FindAsync(id),
            "Moderator" => await _context.Moderators.FindAsync(id),
            "Organizer" => await _context.Organizers.FindAsync(id),
            "Performer" => await _context.Performers.FindAsync(id),
            _ => null
        };
    }

    [HttpPost]
    public async Task<IActionResult> ToggleBlock(string userType, Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        switch (userType)
        {
            case "Customer":
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    customer.Customer_IsBlocked = !customer.Customer_IsBlocked;
                    await _context.SaveChangesAsync();
                }
                break;

            case "InternalCoworker":
                var coworker = await _context.InternalCoworkers.FindAsync(id);
                if (coworker != null && coworker.InternalCoworker_Role != UserRole.Admin)
                {
                    coworker.InternalCoworker_IsBlocked = !coworker.InternalCoworker_IsBlocked;
                    await _context.SaveChangesAsync();
                }
                break;

            case "Location":
                var location = await _context.Locations.FindAsync(id);
                if (location != null)
                {
                    location.Location_IsBlocked = !location.Location_IsBlocked;
                    await _context.SaveChangesAsync();
                }
                break;

            case "Moderator":
                var moderator = await _context.Moderators.FindAsync(id);
                if (moderator != null)
                {
                    moderator.Moderator_IsBlocked = !moderator.Moderator_IsBlocked;
                    await _context.SaveChangesAsync();
                }
                break;

            case "Organizer":
                var organizer = await _context.Organizers.FindAsync(id);
                if (organizer != null)
                {
                    organizer.Organizer_IsBlocked = !organizer.Organizer_IsBlocked;
                    await _context.SaveChangesAsync();
                }
                break;

            case "Performer":
                var performer = await _context.Performers.FindAsync(id);
                if (performer != null)
                {
                    performer.Performer_IsBlocked = !performer.Performer_IsBlocked;
                    await _context.SaveChangesAsync();
                }
                break;
        }

        return RedirectToAction(nameof(UserIndexCoworker), new { userType });
    }

    private string GetUserTypeFromLogin(LogIn login)
    {
        if (login.Customer != null) return "Customer";
        if (login.InternalCoworker != null && login.InternalCoworker.InternalCoworker_Role == UserRole.Admin) return "Admin";
        if (login.InternalCoworker != null) return "Coworker";
        if (login.Location != null) return "Location";
        if (login.Moderator != null) return "Moderator";
        if (login.Organizer != null) return "Organizer";
        if (login.Performer != null) return "Performer";
        return "Unknown";
    }
}
