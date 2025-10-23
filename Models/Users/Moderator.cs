namespace Eventmanagement.Models.Users;

public class Moderator
{
    [Key] public Guid Moderator_Id { get; set; } // Primary Key als GUID
    [ForeignKey("Moderator_Id")] public LogIn Moderator_LogIn { get; set; } // Foreign Key für LogIn
    public UserRole Moderator_Role { get; set; }

    public string Moderator_FirstName { get; set; }
    public string Moderator_LastName { get; set; }
    public DateOnly Moderator_DateOfBirth { get; set; }

    // Kontaktdaten
    public string Moderator_Phone { get; set; }
    public string Moderator_Email { get; set; }
    public string? Moderator_WebSiteURL { get; set; } // Link zur Moderatoren-Webseite


    // Gehalts- und Beschäftigungsdaten
    public decimal Moderator_EventSalary { get; set; }

    public DateTime Moderator_RegistrationDate { get; set; }
    public bool Moderator_IsBlocked { get; set; } = false; // Benutzerstatus

    // Profildaten
    public string? Moderator_ProfileImage { get; set; }
    public string? Moderator_ProfileBio { get; set; }
}
