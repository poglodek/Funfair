using MediatR;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetReservationById;

public record GetReservationByIdCommand(Guid Id) : IRequest<ReservationDto>;