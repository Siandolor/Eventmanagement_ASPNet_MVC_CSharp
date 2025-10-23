namespace Eventmanagement.Models.Events;

public class EventSeatUnit
{
    [Key] public Guid EventSeatUnit_Id { get; set; }

    [ForeignKey("Event")] public Guid Event_Id { get; set; }
    public EventBasics Event { get; set; }

    [ForeignKey("SeatUnit")] public Guid SeatUnit_Id { get; set; }
    public SeatUnit SeatUnit { get; set; }

    // Status: verfügbar, reserviert, verkauft etc.
    public SeatStatus Status { get; set; } = SeatStatus.Available;

    // Preis für diesen Platz bei diesem Event
    [Precision(10, 2)]
    public decimal TicketPrice { get; set; }
    public TicketCategory Category { get; set; }
}
