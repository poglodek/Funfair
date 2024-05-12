using MediatR;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetUserReservation;

public record GetUserReservationCommand(Guid UserId) : IRequest<UserReservationsDto>;