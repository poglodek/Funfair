namespace Reservations.Core.ValueObjects;

public record SeatId(Guid Id);

public record Seat(SeatId Id, string RowNumber, string Number, string SeatClass)
{
    public Seat(Guid id, string rowNumber, string number, string seatClass) : this(new SeatId(id), rowNumber, number, seatClass)
    {
        if (ClassSeat.IsValid(seatClass) is false)
        {
            throw new ArgumentException("Invalid seat class");
        }

        SeatClass = seatClass;
        RowNumber = rowNumber;
        Number = number;
        Id = new SeatId(id);
    }
    
}

public static class ClassSeat
{
    public const string Economy =  nameof(Economy);
    public const string Premium = nameof(Premium);
    public const string Business = nameof(Business);

    public static bool IsValid(string seatClass)
    {
        return seatClass is Economy or Premium or Business;
    }
}
 