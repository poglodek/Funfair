using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;
using Reservations.Core.ValueObjects;

namespace Reservations.Core.Events;

public record NewReservationCreatedEvent(Id Id) : IDomainEvent;

public record NewReservationDraftCreatedEvent(Id Id) : IDomainEvent;
