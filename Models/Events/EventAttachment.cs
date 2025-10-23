namespace Eventmanagement.Models.Events;

public class EventAttachment
{
    [Key] public Guid EventAttachment_Id { get; set; }

    [ForeignKey("Event")]
    public Guid Event_Id { get; set; }
    public EventBasics Event { get; set; }

    [Required]
    public string FileName { get; set; }

    [Required]
    public string MimeType { get; set; }

    [Required]
    public byte[] FileContent { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
