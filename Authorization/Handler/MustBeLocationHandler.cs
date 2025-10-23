namespace Eventmanagement.Authorization.Handlers;

public class MustBeLocationHandler : AuthorizationHandler<MustBeLocationRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeLocationHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeLocationRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var match = _context.Locations.FirstOrDefault(u => u.Location_Id == guid);
        if (match != null && !match.Location_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
