using MediatR;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.CreateReservation;

public record CreateReservationCommand(Guid DraftId, PriceDto Price) : IRequest<ReservationIdDto>;

public record PriceDto(double Value, string Currency)
{
    public static implicit operator Price(PriceDto dto) => new(dto.Value, dto.Currency);
}

public record ReservationIdDto(Guid Value);


