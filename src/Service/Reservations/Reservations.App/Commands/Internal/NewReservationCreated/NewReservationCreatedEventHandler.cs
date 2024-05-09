using Funfair.Messaging.EventHubs.Processor;
using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Events;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.Internal.NewReservationCreated;

internal class NewReservationCreatedEventHandler(IEventProcessor eventProcessor, IReservationRepository repository) : INotificationHandler<NewReservationCreatedEvent>
{
    
    public async Task Handle(NewReservationCreatedEvent notification, CancellationToken cancellationToken)
    {
        var reservation = await repository.GetById(notification.Id, cancellationToken);
        
        if (reservation is null)
        {
            throw new ReservationNotFound($"Reservation with id {notification.Id} not found");
        }

        await eventProcessor.ProcessAsync( new NewReservationCreatedIntegrationEvent
        (
            notification.Id,
            reservation.StandardPrice,
            reservation.Plane.Id,
            reservation.FlightDate,
            reservation.Journey
        ), cancellationToken);
    }
}