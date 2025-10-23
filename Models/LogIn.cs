namespace Eventmanagement.Models;

public class LogIn
{
    [Key] public Guid LogIn_Id { get; set; } // Primary Key, identisch mit User-GUID

    [Required] public string LogIn_Name { get; set; } // E-Mail-Adresse als Benutzername

    [Required] public string LogIn_Password { get; set; } // Gespeichertes Passwort (z. B. gehasht - wird später implementiert)

    // Navigationseigenschaften
    public Customer Customer { get; set; }
    public InternalCoworker InternalCoworker { get; set; }
    public Location Location { get; set; }
    public Moderator Moderator { get; set; }
    public Organizer Organizer { get; set; }
    public Performer Performer { get; set; }

    public virtual UserProfile UserProfile { get; set; }
}
