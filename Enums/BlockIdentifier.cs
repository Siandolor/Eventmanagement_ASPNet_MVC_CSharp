namespace Eventmanagement.Enums;

public enum BlockIdentifier
{
    [Display(Name = "Keine")] Unspecified = 0,
    [Display(Name = "Block")] Block = 1,
    [Display(Name = "Halle")] Hall = 2,
    [Display(Name = "Saal")] Auditorium = 3,
    [Display(Name = "Raum")] Room = 4,
    [Display(Name = "Bühne")] Stage = 5,
}
