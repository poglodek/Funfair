using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core.Events;
using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.UpdateDraft;

public class UpdateDepartureDraftCommandHandler(IReservationRepository reservationRepository, IEventDispatcher eventDispatcher, IUserContextAccessor userContextAccessor)
    : IRequestHandler<UpdateDepartureDraftCommand,Unit>
{
    private const string RequiredClaim = "Worker";
    
    public async Task<Unit> Handle(UpdateDepartureDraftCommand request, CancellationToken cancellationToken)
    {
        userContextAccessor.CheckIfUserHasClaim(RequiredClaim);
        
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);

        if (draft is null)
        {
            throw new DraftNotFoundException(request.Id);
        }
        
        draft.ChangeDeparture(request.Departure);
        
        await reservationRepository.Update(draft, cancellationToken);
        await eventDispatcher.Publish(draft, cancellationToken);
        
        return Unit.Value;
    }
}