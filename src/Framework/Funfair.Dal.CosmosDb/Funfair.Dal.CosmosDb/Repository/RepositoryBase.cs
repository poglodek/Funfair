using System.Net;
using Funfair.Dal.CosmosDb.Model;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Microsoft.Azure.Cosmos;

namespace Funfair.Dal.CosmosDb.Repository;

internal class RepositoryBase<TContainer> : IRepositoryBase<TContainer> where TContainer : ContainerContext
{
    private readonly IClock _clock;
    private readonly Container _containerContext;

    public RepositoryBase(TContainer containerContext, IClock clock)
    {
        _clock = clock;
        _containerContext = containerContext.Container;
    }

    public async Task<bool> CreateItemAsync<TItem>(TItem item,
        PartitionKey? partitionKey = null, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = default) where TItem : class, IDomainBase
    {
        
        var dbModel = new DatabaseModel<TItem>
        {
            Updated = _clock.CurrentDateTime,
            Created = _clock.CurrentDateTime,
            Object = item,
            Version = Guid.NewGuid(),
            Id = item.Id.Value

        };

        var result = await _containerContext.CreateItemAsync(dbModel, partitionKey, requestOptions, cancellationToken);
        return result.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created;
    }

    public IOrderedQueryable<TItem> GetItemLinqQueryable<TItem>(bool allowSynchronousQueryExecution = false,
        string continuationToken = null, QueryRequestOptions requestOptions = null,
        CosmosLinqSerializerOptions linqSerializerOptions = null)
    {
        throw new NotImplementedException();
    }
}