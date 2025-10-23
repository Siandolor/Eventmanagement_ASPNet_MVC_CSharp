namespace Eventmanagement.Models.Events;

public class EventBasics
{
    [Key] public Guid Event_Id { get; set; }


    [Required]
    [StringLength(150)]
    public string Title { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public EventCategory Category { get; set; }
    public EventType Type { get; set; }

    [ForeignKey("Location")] public Guid Location_Id { get; set; }
    public Location Location { get; set; }

    [ForeignKey("Organizer")] public Guid Organizer_Id { get; set; }
    public Organizer Organizer { get; set; }

    // Verbindung zu Sitzplätzen über Join-Tabelle (darin kann dann festgelegt werden, ob gebucht, reserviert, ausgewählt, etc....
    public List<EventSeatUnit> EventSeatUnits { get; set; }
    public int? MaxParticipants { get; set; }

    public List<EventAttachment>? Attachments { get; set; }
    public List<EventSession>? Sessions { get; set; }

    public DateOnly StartDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly EndTime { get; set; }

    public bool IsPublished { get; set; } = false;
    public bool TicketSaleOpen { get; set; } = false;
}
