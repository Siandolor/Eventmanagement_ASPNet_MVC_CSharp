namespace Eventmanagement.Enums;

public enum SeatStatus
{
    [Display(Name = "Verfügbar")] Available = 0,
    [Display(Name = "Reserviert")] Reserved = 1,
    [Display(Name = "Verkauft")] Sold = 2,
    [Display(Name = "Gesperrt")] Blocked = 3
}
