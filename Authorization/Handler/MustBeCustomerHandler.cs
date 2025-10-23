namespace Eventmanagement.Authorization.Handlers;

public class MustBeCustomerHandler : AuthorizationHandler<MustBeCustomerRequirement>
{
    private readonly EventmanagementContext _context;

    public MustBeCustomerHandler(EventmanagementContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeCustomerRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !Guid.TryParse(userId, out var guid)) return Task.CompletedTask;

        var match = _context.Customers.FirstOrDefault(u => u.Customer_Id == guid);
        if (match != null && !match.Customer_IsBlocked)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
