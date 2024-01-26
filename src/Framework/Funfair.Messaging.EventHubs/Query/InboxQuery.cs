using Funfair.Messaging.EventHubs.OutInBoxPattern;
using Funfair.Messaging.EventHubs.OutInBoxPattern.Models;
using Microsoft.Azure.Cosmos;

namespace Funfair.Messaging.EventHubs.Query;

internal interface IInboxQuery
{
    Task<List<Inbox>> GetUnprocessedInboxesAsync(CancellationToken cancellationToken);
}

public class InboxQuery : IInboxQuery
{
    private readonly Container _container;

    public InboxQuery(InOutBoxContainer container)
    {
        _container = container.Container;
    }
    
    public async Task<List<Inbox>> GetUnprocessedInboxesAsync(CancellationToken cancellationToken)
    {
        var query = _container.GetItemQueryIterator<Inbox>(
            new QueryDefinition(Queries.InBoxQuery));

        var results = new List<Inbox>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);
            results.AddRange(response.Resource);
        }

        return results;
    }
}