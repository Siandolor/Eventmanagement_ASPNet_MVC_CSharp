namespace Eventmanagement.Authorization.Handlers;

public class MustBeCoworkerHandler : AuthorizationHandler<MustBeCoworkerRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeCoworkerHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeCoworkerRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid))
            return Task.CompletedTask;

        var match = _context.InternalCoworkers.FirstOrDefault(u => u.InternalCoworker_Id == guid);
        if (match != null && !match.InternalCoworker_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
