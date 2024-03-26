using Funfair.Shared.Core.Events;

namespace Reservations.Core.Events;

public record NewUserReservationEvent(Guid Id, Guid UserId, Guid SeatId) : IDomainEvent;