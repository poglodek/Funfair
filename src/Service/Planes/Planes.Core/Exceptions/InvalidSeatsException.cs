using Funfair.Shared.Exception;

namespace Planes.Core.Exceptions;

public class InvalidSeatsException(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_seats";
}