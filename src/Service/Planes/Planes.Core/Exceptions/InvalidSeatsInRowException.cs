using Funfair.Shared.Exception;

namespace Planes.Core.Exceptions;

public class InvalidSeatsInRowException(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_seats_in_row";

    public static void IfMoreThan(int max, int seatsInRow)
    {
        if(seatsInRow > max)
            throw new InvalidSeatsInRowException($"Seats in row cannot be more than {max}");
    }    
}