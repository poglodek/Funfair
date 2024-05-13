using MediatR;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.UpdateDraft;

public record UpdatePlaneDraftCommand(Guid Id, Guid PlaneId) : UpdateDraftCommandBase, IRequest<Unit>;
public record UpdateDestinyDraftCommand(Guid Id, Airport Destiny) : UpdateDraftCommandBase, IRequest<Unit>;
public record UpdateDateDraftCommand(Guid Id, FlightDate FlightDate) : UpdateDraftCommandBase, IRequest<Unit>;

public abstract record UpdateDraftCommandBase();