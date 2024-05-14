using Flights.Infrastructure.Dto;
using MediatR;

namespace Flights.Infrastructure.Query;

public record GetFlightCommand(string DepartureIAta, string ArrivalIAta, DateTimeOffset Departure) 
    : IRequest<FlightsDto>;