using Funfair.Messaging.EventHubs.Models;
using MediatR;

namespace Flights.App.ExternalEvents;

[Event("new-reservation")]
public record NewReservationCreatedIntegrationEvent(
    Guid Id,
    PriceDto StandardPrice,
    Guid PlaneId,
    FlightDateDto FlightDate,
    JourneyDto Journey) : INotification;

public record PriceDto(double Value, string Currency);

public record FlightDateDto(DateTimeOffset Departure, DateTimeOffset Arrival);

public record JourneyDto(string DepartureIAta, string ArrivalIAta);