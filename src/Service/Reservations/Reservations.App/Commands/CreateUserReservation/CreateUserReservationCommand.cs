using MediatR;

namespace Reservations.App.Commands.CreateUserReservation;

public record CreateUserReservationCommand(Guid UserId, Guid SeatId, Guid ReservationId) : IRequest<Unit>;