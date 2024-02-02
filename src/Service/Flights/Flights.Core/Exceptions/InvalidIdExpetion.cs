using Funfair.Shared.Exception;

namespace Flights.Core.Exceptions;

public class InvalidIdException(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_id";
}