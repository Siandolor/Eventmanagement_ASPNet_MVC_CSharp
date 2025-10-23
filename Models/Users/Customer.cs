namespace Eventmanagement.Models.Users;

public class Customer
{
    [Key] public Guid Customer_Id { get; set; } // Primary Key als GUID
    [ForeignKey("Customer_Id")] public LogIn Customer_LogIn { get; set; } // Foreign Key für LogIn

    public UserRole Customer_Role { get; set; }

    public string Customer_FirstName { get; set; }
    public string Customer_LastName { get; set; }
    public DateOnly Customer_DateOfBirth { get; set; }

    // Adressdaten
    public string Customer_StreetName { get; set; }
    public string Customer_StreetNumber { get; set; }
    public string? Customer_ApartmentNumber { get; set; }
    public string Customer_ZIP { get; set; }
    public string Customer_City { get; set; }
    public string? Customer_State { get; set; }
    public string Customer_Country { get; set; }

    // Kontaktdaten
    public string Customer_Phone { get; set; }
    public string Customer_Email { get; set; }

    public DateTime Customer_RegistrationDate { get; set; }
    public bool Customer_IsBlocked { get; set; } = false; // Benutzerstatus

    // Profildaten
    public string? Customer_ProfileImage { get; set; }
    public string? Customer_ProfileBio { get; set; }
}
