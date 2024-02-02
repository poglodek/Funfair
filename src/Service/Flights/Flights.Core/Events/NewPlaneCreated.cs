using Flights.Core.ValueObjects;
using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;

namespace Flights.Core.Events;

public record NewPlaneCreated(Id PlaneId, Model PlaneModel, IReadOnlyCollection<Seat> PlaneSeats) : IDomainEvent;