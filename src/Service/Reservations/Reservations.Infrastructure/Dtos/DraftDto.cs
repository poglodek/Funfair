namespace Reservations.Infrastructure.Dtos;

public record DraftDto(Guid Id, JourneyDto Journey, FlightDateDto FlightDate, Guid WorkerId, Guid PlaneId);
