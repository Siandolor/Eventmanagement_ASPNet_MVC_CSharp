namespace Eventmanagement.Enums;

public enum TicketStatus
{
    [Display(Name = "Intern")] Draft = 0,
    [Display(Name = "Reserviert")] Reserved = 1,
    [Display(Name = "Zahlung erwartet")] AwaitingPayment = 2,
    [Display(Name = "Bezahlt")] Paid = 3,
    [Display(Name = "Eingecheckt")] CheckedIn = 4,
    [Display(Name = "Storniert")] Cancelled = 5,
    [Display(Name = "Erstattet")] Refunded = 6
}
