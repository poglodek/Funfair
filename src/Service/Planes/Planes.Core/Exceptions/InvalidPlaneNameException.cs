using Funfair.Shared.Exception;

namespace Planes.Core.Exceptions;

public class InvalidPlaneNameException(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_plane_name";
}