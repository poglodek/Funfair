namespace Planes.Infrastructure.Dto;

public record PlanesDto(List<PlaneInfoDto> PlanesInfo);
public record PlaneInfoDto(Guid Id, string Model);
