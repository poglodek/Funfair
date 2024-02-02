namespace Flights.Core.ValueObjects;

public sealed record RowNumber(int Number)
{
    public static implicit operator int(RowNumber rowNumber)
        => rowNumber.Number;

    public static implicit operator RowNumber(int value)
        => new(value);
}
