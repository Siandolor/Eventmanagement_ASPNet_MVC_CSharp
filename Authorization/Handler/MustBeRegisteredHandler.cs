namespace Eventmanagement.Authorization.Handlers;

public class MustBeRegisteredHandler : AuthorizationHandler<MustBeRegisteredRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeRegisteredHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeRegisteredRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var coworkerMatch = _context.InternalCoworkers.FirstOrDefault(u => u.InternalCoworker_Id == guid);
        if (coworkerMatch != null && !coworkerMatch.InternalCoworker_IsBlocked)
            context.Succeed(requirement);

        var customerMatch = _context.Customers.FirstOrDefault(u => u.Customer_Id == guid);
        if (customerMatch != null && !customerMatch.Customer_IsBlocked)
            context.Succeed(requirement);

        var locationMatch = _context.Locations.FirstOrDefault(u => u.Location_Id == guid);
        if (locationMatch != null && !locationMatch.Location_IsBlocked)
            context.Succeed(requirement);

        var moderatorMatch = _context.Moderators.FirstOrDefault(u => u.Moderator_Id == guid);
        if (moderatorMatch != null && !moderatorMatch.Moderator_IsBlocked)
            context.Succeed(requirement);

        var organizerMatch = _context.Organizers.FirstOrDefault(u => u.Organizer_Id == guid);
        if (organizerMatch != null && !organizerMatch.Organizer_IsBlocked)
            context.Succeed(requirement);

        var performerMatch = _context.Performers.FirstOrDefault(u => u.Performer_Id == guid);
        if (performerMatch != null && !performerMatch.Performer_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
