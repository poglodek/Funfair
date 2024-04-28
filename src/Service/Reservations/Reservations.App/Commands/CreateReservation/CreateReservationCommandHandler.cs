using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.Core;
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
    IEventProcessor eventProcessor)
    : IRequestHandler<CreateReservationCommand, Unit>
{
    public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var draft = await reservationRepository.GetDraftById(request.DraftId,cancellationToken);

        if (draft is null)
        {
            throw new DraftNotFoundException(request.DraftId);
        }

        var reservation = draft.Confirm(
            new Worker(userContextAccessor.Get().UserId),
            new Price(request.Price.Value, request.Price.Currency),
            clock);

        
        await reservationRepository.AddNewReservation(reservation, draft,cancellationToken);
        await eventProcessor.ProcessAsync(reservation, cancellationToken);

        return Unit.Value;
    }
}