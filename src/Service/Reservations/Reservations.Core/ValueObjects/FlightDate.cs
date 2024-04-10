namespace Reservations.Core.ValueObjects;

public record FlightDate(DateTimeOffset Departure, DateTimeOffset Arrival);