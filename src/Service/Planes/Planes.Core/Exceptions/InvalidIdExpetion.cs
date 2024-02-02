using Funfair.Shared.Exception;

namespace Planes.Core.Exceptions;

public class InvalidIdException(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_id";
}