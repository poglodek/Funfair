using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.Domain;
using MediatR;
using Reservations.App.Dtos;
using Reservations.App.Services;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.CreateDraft;

public class CreateDraftCommandHandler(
    IPlaneService planeService,
    IUserContextAccessor userContextAccessor,
    IReservationRepository reservationRepository,
    IEventProcessor eventProcessor)
    : IRequestHandler<CreateDraftCommand, CreateDraftCommandDto>
{
    private const string RequiredClaim = "Worker";

    public async Task<CreateDraftCommandDto> Handle(CreateDraftCommand request, CancellationToken cancellationToken)
    {
        var plane = await planeService.GetById(request.PlaneId, cancellationToken);
        var user = userContextAccessor.Get(RequiredClaim);
        
        var draftId = Guid.NewGuid();
        
        var draft = ReservationDraft.Create(new Id(draftId), new Journey(request.DepartureAirport, request.DestinationAirport), 
            new FlightDate(request.Departure, request.Arrival), new Worker(user.UserId), plane);
        
        await reservationRepository.Create(draft, cancellationToken);
        await eventProcessor.ProcessAsync(draft, cancellationToken);
        
        return new CreateDraftCommandDto(draftId);
    }
}