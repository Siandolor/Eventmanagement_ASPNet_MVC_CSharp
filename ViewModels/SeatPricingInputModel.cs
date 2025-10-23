namespace Eventmanagement.ViewModels
{
    public class SeatPricingInputModel
    {
        public Guid Event_Id { get; set; }

        public Guid SeatUnit_Id { get; set; }

        [Precision(10, 2)]
        public decimal BasePrice { get; set; }

        [Precision(10, 2)]
        public decimal StandingDiscount { get; set; }

        public Dictionary<string, decimal> BlockSurcharges { get; set; } = new();
        public Dictionary<string, decimal> LevelSurcharges { get; set; } = new();
        public Dictionary<string, decimal> BoxSurcharges { get; set; } = new();
        public Dictionary<string, decimal> RowSurcharges { get; set; } = new();

        public TicketCategory Category { get; set; }
        public SeatStatus Status { get; set; }
    }

}
