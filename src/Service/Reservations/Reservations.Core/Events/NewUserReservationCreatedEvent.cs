using Funfair.Shared.Core.Events;

namespace Reservations.Core.Events;

public record NewUserReservationCreatedEvent(Guid Id, Guid UserId, Guid SeatId) : IDomainEvent;