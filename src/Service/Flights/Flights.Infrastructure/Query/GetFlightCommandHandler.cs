using Flights.Infrastructure.Dto;
using Funfair.Dal.Gremlin.GermlinApi;
using MediatR;

namespace Flights.Infrastructure.Query;

public class GetFlightCommandHandler(IGremlinRepository client) : IRequestHandler<GetFlightCommand, FlightsDto>
{
    public async Task<FlightsDto> Handle(GetFlightCommand request, CancellationToken cancellationToken)
    {
        var query = @"
        g.V().has('airport', 'code', fromCode)
         .repeat(outE('flight').has('departureTime', gt('{departureTime:O}')).inV()).until(has('code', toCode))
         .path().by(valueMap(true))";

        var parameters = new Dictionary<string, object>
        {
            { "fromCode", request.Departure },
            { "toCode", request.ArrivalIAta },
            { "departureTime", request.Departure.ToString("O") } // Format ISO 8601
        };

        var result = await client.SubmitAsync<dynamic>(query, parameters, cancellationToken);

        var flights = new List<FlightDto>();

        foreach (var path in result)
        {
            var edges = path.objects;
            for (var i = 1; i < edges.Count; i += 2)
            {
                var edge = edges[i];
                var properties = edge as IDictionary<string, object>;

                var reservationId = Guid.Parse(properties["reservationId"].ToString());
                var departureIata = (string)properties["fromCode"];
                var arrivalIata = (string)properties["toCode"];
                var departure = DateTimeOffset.Parse((string)properties["departureTime"]);
                var arrival = DateTimeOffset.Parse((string)properties["arrivalTime"]);

                var flight = new FlightDto(
                    ReservationId: reservationId,
                    DepartureIAta : departureIata,
                    ArrivalIAta: arrivalIata,
                    Departure: departure,
                    Arrival: arrival
                );

                flights.Add(flight);
            }
        }

        return new FlightsDto(flights);
    }
}