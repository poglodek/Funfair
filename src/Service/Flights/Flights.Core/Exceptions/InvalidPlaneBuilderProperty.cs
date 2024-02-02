using Funfair.Shared.Exception;

namespace Flights.Core.Exceptions;

public class InvalidPlaneBuilderProperty(string value) : CoreException(value)
{
    private readonly string _value = value;
    public override string ErrorMessage => $"invalid_plane_builder_property_{_value}";
}