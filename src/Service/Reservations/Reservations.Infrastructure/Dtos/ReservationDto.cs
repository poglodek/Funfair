namespace Reservations.Infrastructure.Dtos;

public record ReservationDto(Guid Id, JourneyDto Journey, FlightDateDto FlightDate,
    Guid WorkerId, Guid PlaneId, PriceDto StandardPrice, List<TakenSeatDto> TakenSeats);