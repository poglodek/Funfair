using Funfair.Messaging.EventHubs.Processor;
using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.UpdateDraft;

public class UpdateDestinyDraftCommandHandler(IReservationRepository reservationRepository, IEventProcessor eventProcessor)
    : IRequestHandler<UpdateDestinyDraftCommand,Unit>
{
    public async Task<Unit> Handle(UpdateDestinyDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);
        
        if (draft is null)
        {
            throw new DraftNotFoundException(request.Id);
        }
        
        draft.ChangeDestiny(request.Destiny);
        
        await reservationRepository.Update(draft, cancellationToken);
        await eventProcessor.ProcessAsync(draft, cancellationToken);
        
        return Unit.Value;
    }
}