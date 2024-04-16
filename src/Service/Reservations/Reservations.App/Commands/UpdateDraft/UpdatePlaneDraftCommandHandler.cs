using MediatR;
using Reservations.App.Exceptions;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.UpdateDraft;

public class UpdatePlaneDraftCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<UpdatePlaneDraftCommand, Unit>
{
    public async Task<Unit> Handle(UpdatePlaneDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);

        if (draft is null)
        {
            throw new DraftNotFoundException(request.Id);
        }
        
        draft.UpdatePlane(request.Plane);
        
        
        await reservationRepository.Update(draft, cancellationToken);

        return Unit.Value;
    }
}