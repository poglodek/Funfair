using Funfair.Shared.Core.Events;

namespace Reservations.Core.Events;

public record UserReservationAssignedReservationEvents(Guid ReservationId, Guid UserReservationId): IDomainEvent;
public record UserReservationRemovedFromReservationEvents(Guid ReservationId, Guid UserReservationId): IDomainEvent;