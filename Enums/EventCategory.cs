namespace Eventmanagement.Enums;

public enum EventCategory
{
    [Display(Name = "Konzert")] Concert = 0,
    [Display(Name = "Theater")] Theater = 1,
    [Display(Name = "Ausstellung")] Exhibition = 2,
    [Display(Name = "Festival")] Festival = 3,
    [Display(Name = "Lesung & Literatur")] Reading = 4,
    [Display(Name = "Tanz")] Dance = 5,
    [Display(Name = "Film & Kino")] Film = 6,
    [Display(Name = "Comedy & Kabarett")] Comedy = 7,
    [Display(Name = "Sportveranstaltung")] Sports = 8,
    [Display(Name = "Workshop & Seminar")] Workshop = 9,
    [Display(Name = "Messe & Markt")] Fair = 10,
    [Display(Name = "Networking & Business")] Business = 11,
    [Display(Name = "Kinder & Familie")] Family = 12,
    [Display(Name = "Charity & Soziales")] Charity = 13,
    [Display(Name = "Sonstige")] Other = 14
}
