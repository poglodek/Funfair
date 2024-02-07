using Funfair.Shared.App;

namespace Planes.Infrastructure.Exceptions;

public class PlaneNotFoundException(string value) : AppException(value)
{
    public override string ErrorMessage => "plane_not_found";
}