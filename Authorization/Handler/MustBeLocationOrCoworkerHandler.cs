namespace Eventmanagement.Authorization.Handlers;

public class MustBeLocationOrCoworkerHandler : AuthorizationHandler<MustBeLocationOrCoworkerRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeLocationOrCoworkerHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeLocationOrCoworkerRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var coworkerMatch = _context.InternalCoworkers.FirstOrDefault(u => u.InternalCoworker_Id == guid);
        if (coworkerMatch != null && !coworkerMatch.InternalCoworker_IsBlocked)
            context.Succeed(requirement);

        var locationMatch = _context.Locations.FirstOrDefault(u => u.Location_Id == guid);
        if (locationMatch != null && !locationMatch.Location_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
