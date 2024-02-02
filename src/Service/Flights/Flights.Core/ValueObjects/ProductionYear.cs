namespace Flights.Core.ValueObjects;

public record ProductionYear(int Year)
{
    public static implicit operator int(ProductionYear productionYear)
        => productionYear.Year;

    public static implicit operator ProductionYear(int value)
        => new(value);
}