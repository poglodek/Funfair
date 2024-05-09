using Funfair.Shared.App.Events;
using MediatR;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.Internal.NewReservationCreated;

public record NewReservationCreatedIntegrationEvent(
    Guid Id,
    PriceDto StandardPrice,
    Guid PlaneId,
    FlightDateDto FlightDate,
    JourneyDto Journey) : Funfair.Messaging.EventHubs.Events.IIntegrationEvent;

public record PriceDto(double Value, string Currency)
{
    public static implicit operator PriceDto(Price price) => new(price.Value, price.Currency);
}

public record FlightDateDto(DateTimeOffset Departure, DateTimeOffset Arrival)
{
    public static implicit operator FlightDateDto(FlightDate flightDate) => new(flightDate.Departure, flightDate.Arrival);
}

public record JourneyDto(string DepartureIAta, string ArrivalIAta)
{
    public static implicit operator JourneyDto(Journey journey) => new(journey.Departure.IAtaCode, journey.Destination.IAtaCode);
}