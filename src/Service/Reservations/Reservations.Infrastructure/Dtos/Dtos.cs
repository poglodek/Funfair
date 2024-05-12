using Reservations.Core.ValueObjects;

namespace Reservations.Infrastructure.Dtos;

public record JourneyDto(AirportDto Departure, AirportDto Destination)
{
    public static implicit operator JourneyDto(Journey journey) => new (journey.Departure, journey.Destination);
}

public record AirportDto(string Name, string City, string IAtaCode)
{
    public static implicit operator AirportDto(Airport airport) => new (airport.Name, airport.City, airport.IAtaCode);
}

public record FlightDateDto(DateTimeOffset Departure, DateTimeOffset Arrival)
{
    public static implicit operator FlightDateDto(FlightDate flightDate) => new (flightDate.Departure, flightDate.Arrival);
}

public record PriceDto(double Value, string Currency)
{
    public static implicit operator PriceDto(Price price) => new (price.Value, price.Currency);
}

public record TakenSeatDto(Guid Id);