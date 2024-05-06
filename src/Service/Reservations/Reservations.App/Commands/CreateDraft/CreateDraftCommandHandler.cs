using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;
using MediatR;
using Reservations.App.Dtos;
using Reservations.App.Exceptions;
using Reservations.App.Services;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.CreateDraft;

public class CreateDraftCommandHandler(
    IPlaneService planeService,
    IUserContextAccessor userContextAccessor,
    IReservationRepository reservationRepository,
    IEventDispatcher eventDispatcher)
    : IRequestHandler<CreateDraftCommand, CreateDraftCommandDto>
{
    private const string RequiredClaim = "Worker";

    public async Task<CreateDraftCommandDto> Handle(CreateDraftCommand request, CancellationToken cancellationToken)
    {
        var user = userContextAccessor.Get(RequiredClaim);
        
        var plane = await planeService.GetById(request.PlaneId, cancellationToken);

        if (plane is null)
        {
            throw new DraftCannotBeCreatedException("Plane not found");
        }
        
        
        var draftId = Guid.NewGuid();
        
        var draft = ReservationDraft.Create(new Id(draftId), new Journey(request.DepartureAirport, request.DestinationAirport), 
            new FlightDate(request.Departure, request.Arrival), new Worker(user.UserId), plane);
        
        await reservationRepository.Create(draft, cancellationToken);
        await eventDispatcher.Publish(draft, cancellationToken);
        
        return new CreateDraftCommandDto(draftId);
    }
}