namespace Eventmanagement.Enums;

public enum TicketCategory
{
    [Display(Name = "Standard")] Standard = 0,
    [Display(Name = "VIP")] VIP = 1,
    [Display(Name = "Ermäßigt")] Reduced = 2,
    [Display(Name = "Stehplatz")] Standing = 3,
    [Display(Name = "Rollstuhl")] Wheelchair = 4
}
