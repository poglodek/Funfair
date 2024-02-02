using Funfair.Shared.Exception;

namespace Flights.Core.Exceptions;

public class InvalidSeatsException(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_seats";
}