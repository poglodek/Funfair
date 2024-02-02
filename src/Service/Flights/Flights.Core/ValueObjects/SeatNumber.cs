namespace Flights.Core.ValueObjects;

public sealed record SeatNumber(string Number)
{
    public static implicit operator string(SeatNumber seatNumber)
        => seatNumber.Number;

    public static implicit operator SeatNumber(string value)
        => new(value);
}