namespace Planes.Infrastructure.Dto;

public record PlaneDto(Guid Id, string Model, int ProductionYear, List<SeatsDto> Seats);

public record SeatsDto(int Number, int Row,string Class);