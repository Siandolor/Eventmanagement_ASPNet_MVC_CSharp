namespace Eventmanagement.Authorization.Handlers;

public class MustBeOrganizerHandler : AuthorizationHandler<MustBeOrganizerRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeOrganizerHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeOrganizerRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var match = _context.Organizers.FirstOrDefault(u => u.Organizer_Id == guid);
        if (match != null && !match.Organizer_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
