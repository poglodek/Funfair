using MediatR;
using Reservations.App.Dtos;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.CreateDraft;

public record CreateDraftCommand(AirportDto DepartureAirport, AirportDto DestinationAirport, 
    Guid PlaneId,DateTimeOffset Departure, DateTimeOffset Arrival ) : IRequest<CreateDraftCommandDto>;

public record AirportDto(string Name, string City, string IAtaCode)
{
    public static implicit operator Airport(AirportDto dto) => new(dto.Name, dto.City, dto.IAtaCode);
};