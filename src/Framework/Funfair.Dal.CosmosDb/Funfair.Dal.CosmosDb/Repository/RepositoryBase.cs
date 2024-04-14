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

    public async Task<bool> CreateItemAsync<TItem>(TItem item,
        PartitionKey? partitionKey = null, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = default) where TItem : class, IDomainBase
    {
        
        var dbModel = new DatabaseModel<TItem>
        {
            Updated = clock.CurrentDateTime,
            Created = clock.CurrentDateTime,
            Object = item,
            Version = Guid.NewGuid(),
            Id = item.Id.Value

        };

        var result = await _containerContext.CreateItemAsync(dbModel, partitionKey, requestOptions, cancellationToken);
        
        return result.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created or HttpStatusCode.Accepted;
    }

    public IQueryable<TItem> GetItemLinqQueryable<TItem>(bool allowSynchronousQueryExecution = false,
        string continuationToken = null, QueryRequestOptions requestOptions = null,
        CosmosLinqSerializerOptions linqSerializerOptions = null)
    {
        return _containerContext.GetItemLinqQueryable<DatabaseModel<TItem>>(allowSynchronousQueryExecution, continuationToken, requestOptions, linqSerializerOptions)
            .Select(x => x.Object);
    }

    public Task<TItem?> GetBytId<TItem>(Id id, CancellationToken cancellationToken) where TItem : class, IDomainBase
    {
        return _containerContext.GetItemLinqQueryable<DatabaseModel<TItem>>().Where(x => x.Id == id.Value)
            .Select(x => x.Object)
            .ToFeedIterator()
            .FirstOrDefaultAsync(cancellationToken);
    }
}