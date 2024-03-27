namespace Planes.Core.ValueObjects;

public record SeatId(Guid Id)
{
    public static implicit operator SeatId (Guid guid) => new (guid);
}


public record Seat(SeatId Id, RowNumber RowNumber, SeatNumber Number, SeatClass SeatClass);