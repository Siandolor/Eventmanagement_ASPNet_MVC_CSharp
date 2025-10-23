namespace Eventmanagement.Enums;

public enum UserRole
{
    [Display(Name = "Nicht eingeloggt")] Unknown = 0,
    [Display(Name = "Ticketkäufer")] Customer = 1,
    [Display(Name = "Administrator")] Admin = 2,
    [Display(Name = "Mitarbeiter")] Coworker = 3,
    [Display(Name = "Veranstalter")] Organizer = 4,
    [Display(Name = "Veranstaltungsort")] Location = 5,
    [Display(Name = "Veranstaltungsmoderator")] Moderator = 6,
    [Display(Name = "Künster")] Performer = 7
}
