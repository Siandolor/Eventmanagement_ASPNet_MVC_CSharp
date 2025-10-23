namespace Eventmanagement.Utilities;

public class SeatUnitBulkCreator
{
    public Guid Location_Id { get; set; }
    public string? BlockName { get; set; }
    public BlockIdentifier BlockType { get; set; }
    public PositionType? Block_Position { get; set; }

    public LevelType Level { get; set; }
    public PositionType Level_Position { get; set; }

    public BoxIdentifier Box { get; set; }
    public PositionType Box_Position { get; set; }

    public string? RowPrefix { get; set; } // z.B. "A", "B" oder "1"
    public int RowStart { get; set; }
    public int RowEnd { get; set; }

    public int SeatStart { get; set; }
    public int SeatEnd { get; set; }

    public PositionType? Row_Position { get; set; }
    public PositionType? Seat_Position { get; set; }

    public bool IsStandingArea { get; set; } = false;
}
