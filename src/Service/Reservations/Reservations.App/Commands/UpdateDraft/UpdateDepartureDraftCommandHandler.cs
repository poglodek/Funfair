using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.UpdateDraft;

public class UpdateDepartureDraftCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<UpdateDepartureDraftCommand,Unit>
{
    public async Task<Unit> Handle(UpdateDepartureDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);

        if (draft is null)
        {
            throw new DraftNotFoundException(request.Id);
        }
        
        draft.ChangeDeparture(request.Departure);
        
        await reservationRepository.Update(draft, cancellationToken);
        
        return Unit.Value;
    }
}