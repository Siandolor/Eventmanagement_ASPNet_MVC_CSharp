namespace Eventmanagement.Models.Users;

public class Organizer
{
    [Key] public Guid Organizer_Id { get; set; } // Primary Key als GUID
    [ForeignKey("Organizer_Id")] public LogIn Organizer_LogIn { get; set; } // Foreign Key für LogIn
    public UserRole Organizer_Role { get; set; }

    public string Organizer_CompanyName { get; set; }
    public string Organizer_ContactPerson { get; set; }
    public string Organizer_FIN { get; set; }

    // Adressdaten
    public string Organizer_StreetName { get; set; }
    public string Organizer_StreetNumber { get; set; }
    public string? Organizer_ApartmentNumber { get; set; }
    public string Organizer_ZIP { get; set; }
    public string Organizer_City { get; set; }
    public string? Organizer_State { get; set; }
    public string Organizer_Country { get; set; }

    // Kontaktdaten
    public string Organizer_Phone { get; set; }
    public string Organizer_Email { get; set; }
    public string? Organizer_WebSiteURL { get; set; } // Link zur Firmen-Webseite

    public DateTime Organizer_RegistrationDate { get; set; }
    public bool Organizer_IsBlocked { get; set; } = false; // Benutzerstatus

    // Profildaten
    public string? Organizer_ProfileImage { get; set; }
    public string? Organizer_ProfileBio { get; set; }
}
