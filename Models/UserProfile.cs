namespace Eventmanagement.Models;

public class UserProfile
{
    [Key, ForeignKey("LogIn")]
    public Guid UserProfileId { get; set; }  // identisch mit LogIn_Id

    public string ProfileImagePath { get; set; }

    public string Bio { get; set; }

    // Navigation zur LogIn-Tabelle
    public LogIn LogIn { get; set; }
}
