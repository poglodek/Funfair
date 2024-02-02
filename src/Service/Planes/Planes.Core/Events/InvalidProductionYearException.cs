using Funfair.Shared.Exception;

namespace Planes.Core.Events;

public class InvalidProductionYearException(int value) : CoreException($"year is invalid: {value}")
{
    public override string ErrorMessage => "invalid_production_year";
}