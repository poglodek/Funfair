namespace Reservations.Core.ValueObjects;

public record SeatId(Guid Id);
public record Seat(SeatId Id,string RowNumber, string Number, string SeatClass);