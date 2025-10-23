namespace Eventmanagement.Authorization.Handlers;

public class MustBeOrganizerOrCoworkerHandler : AuthorizationHandler<MustBeOrganizerOrCoworkerRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeOrganizerOrCoworkerHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeOrganizerOrCoworkerRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var coworkerMatch = _context.InternalCoworkers.FirstOrDefault(u => u.InternalCoworker_Id == guid);
        if (coworkerMatch != null && !coworkerMatch.InternalCoworker_IsBlocked)
            context.Succeed(requirement);

        var organizerMatch = _context.Organizers.FirstOrDefault(u => u.Organizer_Id == guid);
        if (organizerMatch != null && !organizerMatch.Organizer_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
