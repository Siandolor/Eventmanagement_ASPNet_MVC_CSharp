namespace Eventmanagement.ViewModels;

public class UserRegistrationAndRoles
{
    public string SelectedRole { get; set; }
    public Customer Customer { get; set; }
    public InternalCoworker Admin { get; set; }
    public InternalCoworker Coworker { get; set; }
    public Location Location { get; set; }
    public Moderator Moderator { get; set; }
    public Organizer Organizer { get; set; }
    public Performer Performer { get; set; }

    public string SharedEmail { get; set; }
    public string ConfirmPassword { get; set; }
}
