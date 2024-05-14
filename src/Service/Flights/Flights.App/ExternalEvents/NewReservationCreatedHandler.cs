using Funfair.Dal.Gremlin.GermlinApi;
using MediatR;

namespace Flights.App.ExternalEvents;

public class NewReservationCreatedHandler(IGremlinRepository client) : INotificationHandler<NewReservationCreatedIntegrationEvent>
{
    
    public async Task Handle(NewReservationCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var query = "g.addV('airport').property('code', code)";
        
        var parameters = new Dictionary<string, object>
        {
            {"code", notification.Journey.DepartureIAta},
        };

        await client.SubmitAsync(query, parameters, cancellationToken);
        
        query =  @"
            g.V().has('airport', 'code', fromCode).as('a')
             .V().has('airport', 'code', toCode).as('b')
             .addE('flight').from('a').to('b')
             .property('flightNumber', flightNumber)
             .property('departureTime', departureTime)
             .property('arrivalTime', arrivalTime)";
        
        parameters = new Dictionary<string, object>
        {
            {"fromCode", notification.Journey.DepartureIAta},
            {"toCode", notification.Journey.ArrivalIAta},
            {"reservationId", notification.Id},
            {"departureTime", notification.FlightDate.Departure},
            {"arrivalTime", notification.FlightDate.Arrival}
        };
        
        await client.SubmitAsync(query, parameters, cancellationToken);
    }
    
}