using Funfair.Messaging.EventHubs.Processor;
using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.UpdateDraft;

public class UpdateDateDraftCommandHandler (IReservationRepository reservationRepository, IEventProcessor eventProcessor) 
    : IRequestHandler<UpdateDateDraftCommand,Unit>
{
    public async Task<Unit> Handle(UpdateDateDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);
        
        if (draft is null)
        {
            throw new DraftNotFoundException(request.Id);
        }
        
        draft.UpdateDates(request.FlightDate);
        
        await reservationRepository.Update(draft, cancellationToken);
        await eventProcessor.ProcessAsync(draft, cancellationToken);
        
        return Unit.Value;
    }
}