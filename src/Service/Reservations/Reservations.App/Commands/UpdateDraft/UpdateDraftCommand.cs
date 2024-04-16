using MediatR;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.UpdateDraft;

public record UpdatePlaneDraftCommand(Guid Id, Plane Plane) : IRequest<Unit>;
public record UpdateDepartureDraftCommand(Guid Id, Airport Departure) : IRequest<Unit>;
public record UpdateDestinyDraftCommand(Guid Id, Airport Destiny) : IRequest<Unit>;
public record UpdateDateDraftCommand(Guid Id, FlightDate FlightDate) : IRequest<Unit>;