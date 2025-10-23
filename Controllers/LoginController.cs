namespace Eventmanagement.Controllers;

public class LoginController : Controller
{
    private readonly EventmanagementContext _context;
    private readonly IMemoryCache _cache;

    public LoginController(EventmanagementContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutTime = TimeSpan.FromMinutes(15);

    private string GetCacheKey(string username) => $"LoginAttempts_{username.ToLower()}";

    [HttpGet]
    [AutoValidateAntiforgeryToken]
    public IActionResult Login()
    {
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, bool rememberMe)
    {
        var user = await _context.LogIns
            .Include(l => l.Customer)
            .Include(l => l.InternalCoworker)
            .Include(l => l.Location)
            .Include(l => l.Moderator)
            .Include(l => l.Organizer)
            .Include(l => l.Performer)
            .FirstOrDefaultAsync(u => u.LogIn_Name == username);

        if (user == null)
        {
            TempData["LoginError"] = "EmailNotFound";
            return RedirectToAction("Index", "Home");
        }

        if (user.Customer?.Customer_IsBlocked == true ||
            user.InternalCoworker?.InternalCoworker_IsBlocked == true ||
            user.Location?.Location_IsBlocked == true ||
            user.Moderator?.Moderator_IsBlocked == true ||
            user.Organizer?.Organizer_IsBlocked == true ||
            user.Performer?.Performer_IsBlocked == true)
        {
            TempData["LoginError"] = "UserBlocked";
            return RedirectToAction("Index", "Home");
        }

        var cacheKey = GetCacheKey(username);

        if (_cache.TryGetValue(cacheKey, out (int count, DateTime timestamp) attempts))
        {
            if (attempts.count >= MaxFailedAttempts && DateTime.UtcNow - attempts.timestamp < LockoutTime)
            {
                TempData["LoginError"] = "TooManyAttempts";
                return RedirectToAction("Index", "Home");
            }
        }

        if (!GenerateHash.VerifyPassword(password, user.LogIn_Password))
        {
            var newCount = (_cache.TryGetValue(cacheKey, out attempts) ? attempts.count + 1 : 1, DateTime.UtcNow);
            _cache.Set(cacheKey, newCount, LockoutTime);
            TempData["LoginError"] = "InvalidPassword";
            return RedirectToAction("Index", "Home");
        }

        _cache.Remove(cacheKey);

        UserRole? role = user.Customer?.Customer_Role
                      ?? user.InternalCoworker?.InternalCoworker_Role
                      ?? user.Location?.Location_Role
                      ?? user.Moderator?.Moderator_Role
                      ?? user.Organizer?.Organizer_Role
                      ?? user.Performer?.Performer_Role;

        if (role == null)
        {
            TempData["LoginError"] = "RoleNotAssigned";
            return RedirectToAction("Index", "Home");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.LogIn_Name),
            new Claim(ClaimTypes.NameIdentifier, user.LogIn_Id.ToString()),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var identity = new ClaimsIdentity(claims, "UserCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("UserCookie", principal, new AuthenticationProperties { IsPersistent = rememberMe });
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("UserCookie");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View("RequestPasswordReset");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            TempData["LoginError"] = "EmailRequired";
            return RedirectToAction("ResetPassword");
        }

        var user = await _context.LogIns.FirstOrDefaultAsync(u => u.LogIn_Name == email);

        if (user == null)
        {
            TempData["LoginError"] = "EmailNotFound";
            return RedirectToAction("ResetPassword");
        }

        var resetToken = Guid.NewGuid().ToString();

        HttpContext.Session.SetString("ResetToken", resetToken);
        HttpContext.Session.SetString("ResetEmail", user.LogIn_Name);

        return RedirectToAction("EnterNewPasswordForm", new { token = resetToken });
    }

    [HttpGet]
    public IActionResult EnterNewPasswordForm(string token)
    {
        var storedToken = HttpContext.Session.GetString("ResetToken");

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(storedToken) || token != storedToken)
        {
            TempData["LoginError"] = "InvalidResetToken";
            return RedirectToAction("Index", "Home");
        }

        ViewBag.ResetToken = token;
        return View("EnterNewPasswordForm");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnterNewPassword(string newPassword, string token)
    {
        var storedToken = HttpContext.Session.GetString("ResetToken");
        var email = HttpContext.Session.GetString("ResetEmail");

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(storedToken) || token != storedToken || string.IsNullOrEmpty(email))
        {
            TempData["LoginError"] = "InvalidResetToken";
            return RedirectToAction("Index", "Home");
        }

        var user = await _context.LogIns.FirstOrDefaultAsync(u => u.LogIn_Name == email);

        if (user == null)
        {
            TempData["LoginError"] = "EmailNotFound";
            return RedirectToAction("Index", "Home");
        }

        user.LogIn_Password = GenerateHash.HashPassword(newPassword);
        _context.Update(user);
        await _context.SaveChangesAsync();

        HttpContext.Session.Remove("ResetToken");
        HttpContext.Session.Remove("ResetEmail");

        TempData["LoginSuccess"] = "PasswordResetSuccess";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
