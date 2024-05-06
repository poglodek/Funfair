using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core;
using Funfair.Shared.Core.Events;
using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.CreateReservation;

public class CreateReservationCommandHandler(
    IReservationRepository reservationRepository,
    IUserContextAccessor userContextAccessor,
    IClock clock,
    IEventDispatcher eventDispatcher)
    : IRequestHandler<CreateReservationCommand, ReservationIdDto>
{
    private const string RequiredClaim = "Worker";
    
    public async Task<ReservationIdDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var draft = await reservationRepository.GetDraftById(request.DraftId,cancellationToken);

        if (draft is null)
        {
            throw new DraftNotFoundException(request.DraftId);
        }

        var reservation = draft.Confirm(
            new Worker(userContextAccessor.Get(RequiredClaim).UserId),
            request.Price,
            clock);

        
        await reservationRepository.AddNewReservation(reservation, cancellationToken);
        await eventDispatcher.Publish(reservation, cancellationToken);

        return new ReservationIdDto(reservation.Id);
    }
}