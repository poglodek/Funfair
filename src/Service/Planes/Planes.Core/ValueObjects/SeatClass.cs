namespace Planes.Core.ValueObjects;

public sealed record SeatClass(string Value)
{
    public const string Economy = nameof(Economy);
    public const string Premium = nameof(Premium);
    public const string Business = nameof(Business);

    public static implicit operator string(SeatClass seatClass)
        => seatClass.Value;
    
    public static implicit operator SeatClass(string value)
        => new(value);
}