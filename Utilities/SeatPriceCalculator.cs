namespace Eventmanagement.Utilities;

public class SeatPriceCalculator
{
    private readonly decimal _basePrice;
    private readonly decimal _standingDiscount;
    private readonly Dictionary<string, decimal> _blockSurcharge;
    private readonly Dictionary<string, decimal> _levelSurcharge;
    private readonly Dictionary<string, decimal> _boxSurcharge;
    private readonly Dictionary<string, decimal> _rowSurcharge;

    public SeatPriceCalculator(
        decimal basePrice,
        decimal standingDiscount = 0m,
        Dictionary<string, decimal>? block = null,
        Dictionary<string, decimal>? level = null,
        Dictionary<string, decimal>? box = null,
        Dictionary<string, decimal>? row = null)
    {
        _basePrice = basePrice;
        _standingDiscount = standingDiscount;
        _blockSurcharge = block ?? new();
        _levelSurcharge = level ?? new();
        _boxSurcharge = box ?? new();
        _rowSurcharge = row ?? new();
    }

    public decimal CalculatePrice(SeatUnit seat)
    {
        decimal price = _basePrice;

        if (seat.IsStandingArea)
        {
            price -= _standingDiscount;
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(seat.BlockName) &&
                _blockSurcharge.TryGetValue(seat.BlockName, out var block))
                price += block;

            if (_levelSurcharge.TryGetValue(seat.Level.ToString(), out var level))
                price += level;

            if (_boxSurcharge.TryGetValue(seat.Box.ToString(), out var box))
                price += box;

            if (!string.IsNullOrWhiteSpace(seat.Row) &&
                _rowSurcharge.TryGetValue(seat.Row, out var row))
                price += row;
        }

        return price;
    }
}
