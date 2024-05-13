using Reservations.App.Commands.CreateReservation;

namespace Reservations.Api.Requests;


public record CreateReservationRequest(PriceRequest Price);
public record PriceRequest(double Value, string Currency)
{
    public static implicit operator PriceDto(PriceRequest dto) => new(dto.Value, dto.Currency);
}