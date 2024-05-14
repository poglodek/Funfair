namespace Flights.Infrastructure.Dto;

public record FlightsDto(List<FlightDto> Flights);

public record FlightDto(Guid ReservationId, string DepartureIAta, string ArrivalIAta, DateTimeOffset Departure, DateTimeOffset Arrival);
