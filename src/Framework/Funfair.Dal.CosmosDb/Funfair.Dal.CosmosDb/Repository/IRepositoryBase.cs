using Funfair.Dal.CosmosDb.Model;
using Funfair.Shared.Domain;
using Microsoft.Azure.Cosmos;

namespace Funfair.Dal.CosmosDb.Repository;

public interface IRepositoryBase<TContainer> where TContainer : ContainerContext
{
    Task<bool> CreateItemAsync<TItem>(TItem item, PartitionKey? partitionKey = null, string? type = null, 
        ItemRequestOptions requestOptions = null, CancellationToken cancellationToken = default) where TItem : class, IDomainBase;
    IQueryable<TItem> GetItemLinqQueryable<TItem>(string? type = null, bool allowSynchronousQueryExecution = false, string continuationToken = null,  QueryRequestOptions requestOptions = null,  CosmosLinqSerializerOptions linqSerializerOptions = null);
    
    Task<TItem?> GetBytId<TItem>(Id id, string? type = null, CancellationToken cancellationToken = default) where TItem : class, IDomainBase;
}