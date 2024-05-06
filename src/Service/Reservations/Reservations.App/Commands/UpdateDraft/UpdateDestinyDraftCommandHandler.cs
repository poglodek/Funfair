using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core.Events;
using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.UpdateDraft;

public class UpdateDestinyDraftCommandHandler(IReservationRepository reservationRepository, IEventDispatcher eventDispatcher, IUserContextAccessor userContextAccessor)
    : IRequestHandler<UpdateDestinyDraftCommand,Unit>
{
    private const string RequiredClaim = "Worker";
    
    public async Task<Unit> Handle(UpdateDestinyDraftCommand request, CancellationToken cancellationToken)
    {
        userContextAccessor.CheckIfUserHasClaim(RequiredClaim);
        
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);
        
        if (draft is null)
        {
            throw new DraftNotFoundException(request.Id);
        }
        
        draft.ChangeDestiny(request.Destiny);
        
        await reservationRepository.Update(draft, cancellationToken);
        await eventDispatcher.Publish(draft, cancellationToken);
        
        return Unit.Value;
    }
}