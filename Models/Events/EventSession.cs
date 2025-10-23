namespace Eventmanagement.Models.Events;

public class EventSession
{
    [Key] public Guid EventSession_Id { get; set; }

    [ForeignKey("Event")]
    public Guid Event_Id { get; set; }
    public EventBasics Event { get; set; }

    [Required]
    public string Title { get; set; }

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }
    public TimeOnly StartTime { get; set; }

    public DateOnly EndDate { get; set; }
    public TimeOnly EndTime { get; set; }

    public string? Speaker { get; set; }
}
