namespace Eventmanagement.Controllers;

public class HomeController : Controller
{
    private readonly EventmanagementContext _context;

    public HomeController(EventmanagementContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult DebugLogin()
    {
        var logins = _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.Organizer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Moderator)
            .Include(l => l.Performer)
            .ToList();

        return PartialView("_DebugLogin", logins);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
