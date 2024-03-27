using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;

namespace Reservations.Core.Events;

public record NewUserReservationCreatedEvent(Guid Id, Guid UserId, Guid SeatId) : IDomainEvent;