using Funfair.Dal.CosmosDb.Model;
using Funfair.Shared.Domain;
using Microsoft.Azure.Cosmos;

namespace Funfair.Dal.CosmosDb.Repository;

public interface IRepositoryBase<TContainer> where TContainer : ContainerContext
{
    Task<bool> CreateItemAsync<TItem>(TItem item, PartitionKey? partitionKey = null,
        ItemRequestOptions requestOptions = null, CancellationToken cancellationToken = default) where TItem : class, IDomainBase;
    IOrderedQueryable<TItem> GetItemLinqQueryable<TItem>(bool allowSynchronousQueryExecution = false, string continuationToken = null,  QueryRequestOptions requestOptions = null,  CosmosLinqSerializerOptions linqSerializerOptions = null);
}