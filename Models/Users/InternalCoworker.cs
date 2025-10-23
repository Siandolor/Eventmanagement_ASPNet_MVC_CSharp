namespace Eventmanagement.Models.Users;

public class InternalCoworker
{
    [Key] public Guid InternalCoworker_Id { get; set; } // Primary Key als GUID
    [ForeignKey("InternalCoworker_Id")] public LogIn InternalCoworker_LogIn { get; set; } // Foreign Key für LogIn
    public UserRole InternalCoworker_Role { get; set; } // Admin oder Coworker

    public string InternalCoworker_FirstName { get; set; }
    public string InternalCoworker_LastName { get; set; }
    public DateOnly InternalCoworker_DateOfBirth { get; set; }

    // Adressdaten
    public string InternalCoworker_StreetName { get; set; }
    public string InternalCoworker_StreetNumber { get; set; }
    public string? InternalCoworker_ApartmentNumber { get; set; }
    public string InternalCoworker_ZIP { get; set; }
    public string InternalCoworker_City { get; set; }
    public string? InternalCoworker_State { get; set; }
    public string InternalCoworker_Country { get; set; }

    // Kontaktdaten
    public string InternalCoworker_Phone { get; set; }
    public string InternalCoworker_Email { get; set; }

    // Gehalts- und Beschäftigungsdaten
    public decimal InternalCoworker_MonthlySalary { get; set; }
    public DateTime InternalCoworker_EntryDate { get; set; }
    public DateTime? InternalCoworker_ExitDate { get; set; }

    public DateTime InternalCoworker_RegistrationDate { get; set; }
    public bool InternalCoworker_IsBlocked { get; set; } = false; // Benutzerstatus

    // Profildaten
    public string? InternalCoworker_ProfileImage { get; set; }
    public string? InternalCoworker_ProfileBio { get; set; }
}
