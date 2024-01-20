using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Microsoft.Azure.Cosmos;

namespace Funfair.Messaging.AzureServiceBus.Query;

internal interface IOutboxQuery
{
    Task<List<Outbox>> GetUnprocessedInboxesAsync(CancellationToken cancellationToken);
}

internal class OutboxQuery : IOutboxQuery
{
    private readonly Container _container;

    public OutboxQuery(Container container)
    {
        _container = container;
    }
    
    public async Task<List<Outbox>> GetUnprocessedInboxesAsync(CancellationToken cancellationToken)
    {
        var query = _container.GetItemQueryIterator<Outbox>(
            new QueryDefinition(Queries.OutBoxQuery));

        var results = new List<Outbox>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);
            results.AddRange(response.Resource);
        }

        return results;
    }
}