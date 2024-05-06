using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core.Events;
using MediatR;
using Reservations.App.Exceptions;
using Reservations.App.Services;
using Reservations.Core.Repository;

namespace Reservations.App.Commands.UpdateDraft;

public class UpdatePlaneDraftCommandHandler(IReservationRepository reservationRepository, IEventDispatcher eventDispatcher, IUserContextAccessor userContextAccessor, IPlaneService planeService)
    : IRequestHandler<UpdatePlaneDraftCommand, Unit>
{
    private const string RequiredClaim = "Worker";
    
    public async Task<Unit> Handle(UpdatePlaneDraftCommand request, CancellationToken cancellationToken)
    {
        userContextAccessor.CheckIfUserHasClaim(RequiredClaim);
        
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);

        if (draft is null)
        {
            throw new DraftNotFoundException(request.Id);
        }
        
        var plane = await planeService.GetById(request.PlaneId, cancellationToken);

        if (plane is null)
        {
            throw new DraftCannotBeCreatedException("Plane not found");
        }
        
        draft.UpdatePlane(plane);
        
        
        await reservationRepository.Update(draft, cancellationToken);
        await eventDispatcher.Publish(draft, cancellationToken);

        return Unit.Value;
    }
}