using MediatR;

namespace Reservations.App.Commands.CreateReservation;

public record CreateReservationCommand(Guid DraftId, PriceDto Price) : IRequest<Unit>;

public record PriceDto(double Value, string Currency);
