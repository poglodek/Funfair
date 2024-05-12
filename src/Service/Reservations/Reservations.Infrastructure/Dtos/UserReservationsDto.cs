namespace Reservations.Infrastructure.Dtos;

public record UserReservationsDto(List<UserReservationDto> Reservations);

public record UserReservationDto(Guid ReservationId, Guid UserReservationId);