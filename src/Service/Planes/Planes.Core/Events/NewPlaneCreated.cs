using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;
using Planes.Core.ValueObjects;

namespace Planes.Core.Events;

public record NewPlaneCreated(Id PlaneId, Model PlaneModel, IReadOnlyCollection<Seat> PlaneSeats) : IDomainEvent;