namespace Eventmanagement.Authorization.Handlers;

public class MustBeModeratorHandler : AuthorizationHandler<MustBeModeratorRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeModeratorHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeModeratorRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var match = _context.Moderators.FirstOrDefault(u => u.Moderator_Id == guid);
        if (match != null && !match.Moderator_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
