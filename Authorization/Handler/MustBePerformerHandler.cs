namespace Eventmanagement.Authorization.Handlers;

public class MustBePerformerHandler : AuthorizationHandler<MustBePerformerRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBePerformerHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBePerformerRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var match = _context.Performers.FirstOrDefault(u => u.Performer_Id == guid);
        if (match != null && !match.Performer_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
