using System.Net;
using Funfair.Dal.CosmosDb.Linq;
using Funfair.Dal.CosmosDb.Model;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Funfair.Dal.CosmosDb.Repository;

internal class RepositoryBase<TContainer>(TContainer containerContext, IClock clock) : IRepositoryBase<TContainer>
    where TContainer : ContainerContext
{
    private readonly Container _containerContext = containerContext.Container;

    public async Task<bool> CreateItemAsync<TItem>(TItem item, PartitionKey? partitionKey = null, string? type = null,
        ItemRequestOptions requestOptions = null, CancellationToken cancellationToken = default) where TItem : class, IDomainBase

    {
        if (string.IsNullOrEmpty(type))
        {
            type = string.Empty;
        }
        
        var dbModel = new DatabaseModel<TItem>
        {
            Updated = clock.CurrentDateTime,
            Created = clock.CurrentDateTime,
            Object = item,
            Type = type,
            Version = Guid.NewGuid(),
            Id = item.Id.Value

        };

        var result = await _containerContext.CreateItemAsync(dbModel, partitionKey, requestOptions, cancellationToken);
        
        return result.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created or HttpStatusCode.Accepted;
    }
    
    public IQueryable<TItem> GetItemLinqQueryable<TItem>(string? type = null, bool allowSynchronousQueryExecution = true,
        string continuationToken = null, QueryRequestOptions requestOptions = null,
        CosmosLinqSerializerOptions linqSerializerOptions = null)
    {
        var query = _containerContext
            .GetItemLinqQueryable<DatabaseModel<TItem>>(allowSynchronousQueryExecution, continuationToken,
                requestOptions, linqSerializerOptions)
            .Select(x=>x);
        
        if(string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(x => x.Type == type);
        }
        
        return query
                .Select(x => x.Object);

    }

    public Task<TItem?> GetById<TItem>(Id id, string? type = null, CancellationToken cancellationToken = default) where TItem : class, IDomainBase
    {
        var query = _containerContext
            .GetItemLinqQueryable<DatabaseModel<TItem>>()
            .Where(x => x.Id == id.Value);

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(x => x.Type == type);
        }
        
        return query
            .Select(x => x.Object)
            .ToFeedIterator()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task UpsertItemAsync<TItem>(TItem item, PartitionKey? partitionKey = null, string? type = null,
        ItemRequestOptions requestOptions = null, CancellationToken cancellationToken = default) where TItem : class, IDomainBase
    {
        
        if (string.IsNullOrEmpty(type))
        {
            type = string.Empty;
        }
        
        var dbModel = new DatabaseModel<TItem>
        {
            Updated = clock.CurrentDateTime,
            Object = item,
            Type = type,
            Version = Guid.NewGuid(),
            Id = item.Id.Value

        };
        
        return _containerContext.UpsertItemAsync(dbModel, partitionKey, requestOptions, cancellationToken);
    }
    
    public Task DeleteItemAsync<TItem>(TItem item, PartitionKey partitionKey,
        ItemRequestOptions requestOptions = null, CancellationToken cancellationToken = default) where TItem : class, IDomainBase
    {
        return _containerContext.DeleteItemAsync<TItem>(item.Id.Value.ToString(), partitionKey, requestOptions, cancellationToken);
    }
}