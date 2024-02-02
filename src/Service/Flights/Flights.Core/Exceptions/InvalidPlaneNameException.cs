using Funfair.Shared.Exception;

namespace Flights.Core.Exceptions;

public class InvalidPlaneNameException(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_plane_name";
}