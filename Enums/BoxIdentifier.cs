namespace Eventmanagement.Enums;

public enum BoxIdentifier
{
    [Display(Name = "Keine")] Unspecified = 0,

    // Logen 1–12
    [Display(Name = "Loge 1")] Loge_01 = 1,
    [Display(Name = "Loge 2")] Loge_02 = 2,
    [Display(Name = "Loge 3")] Loge_03 = 3,
    [Display(Name = "Loge 4")] Loge_04 = 4,
    [Display(Name = "Loge 5")] Loge_05 = 5,
    [Display(Name = "Loge 6")] Loge_06 = 6,
    [Display(Name = "Loge 7")] Loge_07 = 7,
    [Display(Name = "Loge 8")] Loge_08 = 8,
    [Display(Name = "Loge 9")] Loge_09 = 9,
    [Display(Name = "Loge 10")] Loge_10 = 10,
    [Display(Name = "Loge 11")] Loge_11 = 11,
    [Display(Name = "Loge 12")] Loge_12 = 12,

    // Logen A–N
    [Display(Name = "Loge A")] Loge_A = 13,
    [Display(Name = "Loge B")] Loge_B = 14,
    [Display(Name = "Loge C")] Loge_C = 15,
    [Display(Name = "Loge D")] Loge_D = 16,
    [Display(Name = "Loge E")] Loge_E = 17,
    [Display(Name = "Loge F")] Loge_F = 18,
    [Display(Name = "Loge G")] Loge_G = 19,
    [Display(Name = "Loge H")] Loge_H = 20,
    [Display(Name = "Loge I")] Loge_I = 21,
    [Display(Name = "Loge J")] Loge_J = 22,
    [Display(Name = "Loge K")] Loge_K = 23,
    [Display(Name = "Loge L")] Loge_L = 24,
    [Display(Name = "Loge M")] Loge_M = 25,
    [Display(Name = "Loge N")] Loge_N = 26,

    // Spezielle Logen
    [Display(Name = "Ehrenloge")] Honor = 27,
    [Display(Name = "Technikloge")] Technical = 28,
    [Display(Name = "Dirigentenloge")] Conductor = 29,
    [Display(Name = "Proszeniumsloge")] ProsceniumLeft = 30,
    [Display(Name = "VIP-Loge")] VIP = 31,
    [Display(Name = "Mittelloge")] CenterParterre = 32,
    [Display(Name = "Direktionsloge")] Director = 33,
    [Display(Name = "Diplomatenloge")] Diplomatic = 34
}
