namespace Eventmanagement.Models.Users;

public class Performer
{
    [Key] public Guid Performer_Id { get; set; } // Primary Key als GUID
    [ForeignKey("Performer_Id")] public LogIn Performer_LogIn { get; set; } // Foreign Key für LogIn

    public UserRole Performer_Role { get; set; }

    public string Performer_Name { get; set; }
    public DateOnly Performer_DateOfBirth { get; set; }

    // Kontaktdaten
    public string Performer_Management { get; set; }
    public string Performer_Phone { get; set; }
    public string Performer_Email { get; set; }
    public string? Performer_WebSiteURL { get; set; } // Link zur Performer-Webseite


    // Gehalts- und Beschäftigungsdaten
    public decimal Performer_EventSalary { get; set; }

    public DateTime Performer_RegistrationDate { get; set; }
    public bool Performer_IsBlocked { get; set; } = false; // Benutzerstatus

    // Profildaten
    public string? Performer_ProfileImage { get; set; }
    public string? Performer_ProfileBio { get; set; }
}
