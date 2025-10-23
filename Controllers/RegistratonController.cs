namespace Eventmanagement.Controllers;

public class RegistrationController : Controller
{
    private readonly EventmanagementContext _context;

    public RegistrationController(EventmanagementContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register(string model)
    {
        return View("Register", new UserRegistrationAndRoles
        {
            SelectedRole = model,
            Customer = new Customer { Customer_LogIn = new LogIn() },
            Organizer = new Organizer { Organizer_LogIn = new LogIn() },
            Location = new Location { Location_LogIn = new LogIn() },
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
            return View("Register", model);
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
            return View("Register", model);
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
            return View("Register", model);
        }

        if (model.Organizer.Organizer_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Organizer";
            return View("Register", model);
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
            return View("Register", model);
        }

        if (model.Location.Location_LogIn.LogIn_Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Die Passwörter stimmen nicht überein.");

        if (_context.LogIns.Any(l => l.LogIn_Name == model.SharedEmail))
            ModelState.AddModelError("SharedEmail", "Die E-Mail-Adresse ist bereits registriert.");

        if (!ModelState.IsValid)
        {
            model.SelectedRole = "Location";
            return View("Register", model);
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
}
