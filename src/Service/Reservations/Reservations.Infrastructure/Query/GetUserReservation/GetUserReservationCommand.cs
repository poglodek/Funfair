using MediatR;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetUserReservation;

public record GetUserReservationCommand() : IRequest<UserReservationsDto>;