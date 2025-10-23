namespace Eventmanagement.Models.Tickets
{
    [Index(nameof(TicketNumber), IsUnique = true)]
    public class Ticket
    {
        [Key] public Guid Ticket_Id { get; set; }

        [Required, StringLength(40)]
        public string TicketNumber { get; set; } = default!;

        [ForeignKey(nameof(Event))] public Guid Event_Id { get; set; }
        public EventBasics Event { get; set; } = default!;

        [ForeignKey(nameof(Session))] public Guid? EventSession_Id { get; set; }
        public EventSession? Session { get; set; }

        [ForeignKey(nameof(SeatUnit))] public Guid? EventSeatUnit_Id { get; set; }
        public EventSeatUnit? SeatUnit { get; set; }

        public TicketCategory Category { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }

        [Required, StringLength(3)]
        public string Currency { get; set; } = "EUR";

        public TicketStatus Status { get; set; } = TicketStatus.Reserved;

        public DateTime? ReservationExpiresAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAtUtc { get; set; }
        public DateTime? CancelledAtUtc { get; set; }
        public DateTime? CheckedInAtUtc { get; set; }

        [StringLength(100)] public string? HolderFirstName { get; set; }
        [StringLength(100)] public string? HolderLastName { get; set; }
        [StringLength(200), EmailAddress] public string? HolderEmail { get; set; }
        [StringLength(40)] public string? HolderPhone { get; set; }

        [StringLength(512)]
        public string? CodePayload { get; set; }

        public Guid? Order_Id { get; set; }

        [StringLength(80)] public string? PaymentProvider { get; set; }
        [StringLength(120)] public string? PaymentReference { get; set; }

        [Timestamp] public byte[]? RowVersion { get; set; }
    }
}
