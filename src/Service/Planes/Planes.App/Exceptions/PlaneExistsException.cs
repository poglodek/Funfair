using Funfair.Shared.App;

namespace Planes.App.Exceptions;

public class PlaneExistsException(string value) : AppException(value)
{
    public override string ErrorMessage => "plane_exists";
}