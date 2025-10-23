namespace Eventmanagement.Models.Locations;

public class SeatUnit
{
    [Key] public Guid SeatUnit_Id { get; set; }

    [ForeignKey("Location")] public Guid Location_Id { get; set; }

    // Navigationseigenschaft
    public Location Location { get; set; }

    // Struktur (nullable für Stehplätze)
    public BlockIdentifier BlockType { get; set; }
    public string? BlockName { get; set; }
    public PositionType? Block_Position { get; set; }

    public LevelType Level { get; set; }
    public PositionType Level_Position { get; set; }

    public BoxIdentifier Box { get; set; }
    public PositionType Box_Position { get; set; }

    public string? Row { get; set; }
    public PositionType? Row_Position { get; set; }

    // Sitz-/Stehplatznummer
    [Required]
    public int SeatNumber { get; set; } // 1–99 bei Sitzplätzen, 0 bei Stehplätzen

    public PositionType? Seat_Position { get; set; }

    // Typ
    public bool IsStandingArea { get; set; } = false; // true, wenn SeatNumber = 0
}
