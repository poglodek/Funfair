using Reservations.Core.ValueObjects;

namespace Reservations.Core.Policy;

public class PolicyPrice
{
    private readonly Dictionary<string, double> _priceRatio = new ()
    {
        {ClassSeat.Economy, 1},
        {ClassSeat.Premium, 1.75},
        {ClassSeat.Business, 2.22}
    };
    
    
    public Price CalculatePrice(Seat seat, Price standardPrice)
    {
        ArgumentNullException.ThrowIfNull(seat);
        ArgumentNullException.ThrowIfNull(standardPrice);

        var ratio = _priceRatio[seat.SeatClass];
        var price = standardPrice.Value * ratio;
        return standardPrice with { Value = Math.Round(price,2) };
    }
}