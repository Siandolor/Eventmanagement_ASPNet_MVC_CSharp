namespace Eventmanagement.Controllers;

[Authorize(Policy = "RegisteredOnly")]

public class CommonController : Controller
{
    private readonly EventmanagementContext _context;

    public CommonController(EventmanagementContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> UserIndexCommon(string userType)
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

    public async Task<IActionResult> UserDetailsCommon(string userType, Guid id)
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

        return PartialView("~/Views/Common/_OrganizerEvents.cshtml", events);
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
}
