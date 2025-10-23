namespace Eventmanagement.Enums;

public enum LevelType
{
    [Display(Name = "Keine")] Unspecified = 0,
    [Display(Name = "Circle")] Circle = 1,
    [Display(Name = "Parkett")] Parkett = 2,
    [Display(Name = "Parterre")] Parterre = 3,
    [Display(Name = "1. Rang")] Rang1 = 4,
    [Display(Name = "2. Rang")] Rang2 = 5,
    [Display(Name = "3. Rang")] Rang3 = 6,
    [Display(Name = "4. Rang")] Rang4 = 7,
    [Display(Name = "5. Rang")] Rang5 = 8,
    [Display(Name = "6. Rang")] Rang6 = 9,
    [Display(Name = "7. Rang")] Rang7 = 10,
    [Display(Name = "8. Rang")] Rang8 = 11,
    [Display(Name = "9. Rang")] Rang9 = 12,
    [Display(Name = "10. Rang")] Rang10 = 13,
    [Display(Name = "11. Rang")] Rang11 = 14,
    [Display(Name = "12. Rang")] Rang12 = 15,
    [Display(Name = "Galerie")] Gallery = 16,
    [Display(Name = "Balkon")] Balcony = 17,
    [Display(Name = "Sperrsitz")] RestrictedView = 18,
    [Display(Name = "Proszeniumsloge")] ProsceniumBox = 19,
    [Display(Name = "Dirigentenloge")] ConductorBox = 20,
    [Display(Name = "Unterrang")] LowerTier = 21,
    [Display(Name = "Mittelrang")] MiddleTier = 22,
    [Display(Name = "Oberrang")] UpperTier = 23,
    [Display(Name = "VIP-Logenebene")] VipBoxTier = 24,
    [Display(Name = "Pressetribüne")] PressArea = 25,
    [Display(Name = "Technikebene")] TechnicalLevel = 26
}
