namespace Eventmanagement.Models.Users;

public class Location
{
    [Key] public Guid Location_Id { get; set; } // Primary Key als GUID
    [ForeignKey("Location_Id")] public LogIn Location_LogIn { get; set; } // Foreign Key für LogIn
    public UserRole Location_Role { get; set; }

    public string Location_CompanyName { get; set; }
    public string Location_ContactPerson { get; set; }
    public string Location_FIN { get; set; }

    // Adressdaten
    public string Location_StreetName { get; set; }
    public string Location_StreetNumber { get; set; }
    public string? Location_ApartmentNumber { get; set; }
    public string Location_ZIP { get; set; }
    public string Location_City { get; set; }
    public string? Location_State { get; set; }
    public string Location_Country { get; set; }

    // Kontaktdaten
    public string Location_Phone { get; set; }
    public string Location_Email { get; set; }
    public string? Location_WebSiteURL { get; set; } // Link zur Firmen-Webseite

    public int Location_SeatingsAmount { get; set; } // Anzahl der Sitzplätze
    public int Location_StandingsAmount { get; set; } // Anzahl der Stehplätze

    public DateTime Location_RegistrationDate { get; set; }
    public bool Location_IsBlocked { get; set; } = false; // Benutzerstatus
    public ICollection<SeatUnit> SeatUnits { get; set; } = new List<SeatUnit>();

    // Profildaten
    public string? Location_ProfileImage { get; set; }
    public string? Location_ProfileBio { get; set; }
}
