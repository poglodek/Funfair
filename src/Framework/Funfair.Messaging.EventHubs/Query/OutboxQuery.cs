﻿using Funfair.Messaging.EventHubs.OutInBoxPattern;
using Funfair.Messaging.EventHubs.OutInBoxPattern.Models;
using Microsoft.Azure.Cosmos;

namespace Funfair.Messaging.EventHubs.Query;

internal interface IOutboxQuery
{
    Task<List<Outbox>> GetUnprocessedInboxesAsync(CancellationToken cancellationToken);
}

internal class OutboxQuery : IOutboxQuery
{
    private readonly Container _container;

    public OutboxQuery(InOutBoxContainer container)
    {
        _container = container.Container;
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