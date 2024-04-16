using MediatR;
using Reservations.App.Dtos;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.CreateDraft;

public record CreateDraftCommand(Airport DepartureAirport, Airport DestinationAirport, 
    Guid PlaneId,DateTimeOffset Departure, DateTimeOffset Arrival ) : IRequest<CreateDraftCommandDto>;