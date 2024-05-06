using MediatR;
using Reservations.Core.Events;

namespace Reservations.App.Commands.Internal;

public class NewReservationCreatedEventHandler() : INotificationHandler<NewReservationCreatedEvent>
{
    
    public Task Handle(NewReservationCreatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}