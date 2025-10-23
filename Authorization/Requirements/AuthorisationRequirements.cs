// ============================================================================
// File: AuthorizationRequirements.cs
// Description: Contains all custom authorization requirement marker classes 
//              used across the Event Management System. Each class implements 
//              IAuthorizationRequirement and is used to define specific access 
//              control policies (e.g., CoworkerOnly, OrganizerOnly, etc.).
// ============================================================================

using Microsoft.AspNetCore.Authorization;

namespace Eventmanagement.Authorization.Requirements
{
    // ------------------------------------------------------------------------
    // --- General registration and access requirements
    // ------------------------------------------------------------------------
    public class MustBeRegisteredRequirement : IAuthorizationRequirement { }

    // ------------------------------------------------------------------------
    // --- User role-specific access requirements
    // ------------------------------------------------------------------------
    public class MustBeCustomerRequirement : IAuthorizationRequirement { }

    public class MustBeCoworkerRequirement : IAuthorizationRequirement { }

    public class MustBeOrganizerRequirement : IAuthorizationRequirement { }

    public class MustBePerformerRequirement : IAuthorizationRequirement { }

    public class MustBeModeratorRequirement : IAuthorizationRequirement { }

    public class MustBeLocationRequirement : IAuthorizationRequirement { }

    // ------------------------------------------------------------------------
    // --- Combined role requirements (multiple roles allowed)
    // ------------------------------------------------------------------------
    public class MustBeOrganizerOrCoworkerRequirement : IAuthorizationRequirement { }

    public class MustBeLocationOrCoworkerRequirement : IAuthorizationRequirement { }
}
