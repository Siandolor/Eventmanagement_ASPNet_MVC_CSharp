namespace Eventmanagement.Enums;

public enum EventType
{
    [Display(Name = "Innenbereich")] Indoor = 0,
    [Display(Name = "Open Air")] OpenAir = 1,
    [Display(Name = "Online")] Online = 2,
    [Display(Name = "Hybrid")] Hybrid = 3
}