namespace Eventmanagement.Utilities;

public static class ModelStateCleaner
{
    public static void CleanCustomer(ModelStateDictionary modelState)
    {
        modelState.Remove("Customer.Customer_LogIn.Customer");
        modelState.Remove("Customer.Customer_LogIn.InternalCoworker");
        modelState.Remove("Customer.Customer_LogIn.Location");
        modelState.Remove("Customer.Customer_LogIn.Moderator");
        modelState.Remove("Customer.Customer_LogIn.Organizer");
        modelState.Remove("Customer.Customer_LogIn.Performer");
        modelState.Remove("Customer.Customer_LogIn.UserProfile");
    }

    public static void CleanAdmin(ModelStateDictionary modelState)
    {
        modelState.Remove("Admin.InternalCoworker_LogIn.Customer");
        modelState.Remove("Admin.InternalCoworker_LogIn.InternalCoworker");
        modelState.Remove("Admin.InternalCoworker_LogIn.Location");
        modelState.Remove("Admin.InternalCoworker_LogIn.Moderator");
        modelState.Remove("Admin.InternalCoworker_LogIn.Organizer");
        modelState.Remove("Admin.InternalCoworker_LogIn.Performer");
        modelState.Remove("Admin.InternalCoworker_LogIn.UserProfile");
    }
    public static void CleanCoworker(ModelStateDictionary modelState)
    {
        modelState.Remove("Coworker.InternalCoworker_LogIn.Customer");
        modelState.Remove("Coworker.InternalCoworker_LogIn.InternalCoworker");
        modelState.Remove("Coworker.InternalCoworker_LogIn.Location");
        modelState.Remove("Coworker.InternalCoworker_LogIn.Moderator");
        modelState.Remove("Coworker.InternalCoworker_LogIn.Organizer");
        modelState.Remove("Coworker.InternalCoworker_LogIn.Performer");
        modelState.Remove("Coworker.InternalCoworker_LogIn.UserProfile");
    }

    public static void CleanLocation(ModelStateDictionary modelState)
    {
        modelState.Remove("Location.Location_LogIn.Customer");
        modelState.Remove("Location.Location_LogIn.InternalCoworker");
        modelState.Remove("Location.Location_LogIn.Location");
        modelState.Remove("Location.Location_LogIn.Moderator");
        modelState.Remove("Location.Location_LogIn.Organizer");
        modelState.Remove("Location.Location_LogIn.Performer");
        modelState.Remove("Location.Location_LogIn.UserProfile");
    }

    public static void CleanModerator(ModelStateDictionary modelState)
    {
        modelState.Remove("Moderator.Moderator_LogIn.Customer");
        modelState.Remove("Moderator.Moderator_LogIn.InternalCoworker");
        modelState.Remove("Moderator.Moderator_LogIn.Location");
        modelState.Remove("Moderator.Moderator_LogIn.Moderator");
        modelState.Remove("Moderator.Moderator_LogIn.Organizer");
        modelState.Remove("Moderator.Moderator_LogIn.Performer");
        modelState.Remove("Moderator.Moderator_LogIn.UserProfile");
    }

    public static void CleanOrganizer(ModelStateDictionary modelState)
    {
        modelState.Remove("Organizer.Organizer_LogIn.Customer");
        modelState.Remove("Organizer.Organizer_LogIn.InternalCoworker");
        modelState.Remove("Organizer.Organizer_LogIn.Location");
        modelState.Remove("Organizer.Organizer_LogIn.Moderator");
        modelState.Remove("Organizer.Organizer_LogIn.Organizer");
        modelState.Remove("Organizer.Organizer_LogIn.Performer");
        modelState.Remove("Organizer.Organizer_LogIn.UserProfile");
    }

    public static void CleanPerformer(ModelStateDictionary modelState)
    {
        modelState.Remove("Performer.Performer_LogIn.Customer");
        modelState.Remove("Performer.Performer_LogIn.InternalCoworker");
        modelState.Remove("Performer.Performer_LogIn.Location");
        modelState.Remove("Performer.Performer_LogIn.Moderator");
        modelState.Remove("Performer.Performer_LogIn.Organizer");
        modelState.Remove("Performer.Performer_LogIn.Performer");
        modelState.Remove("Performer.Performer_LogIn.UserProfile");
    }
}
