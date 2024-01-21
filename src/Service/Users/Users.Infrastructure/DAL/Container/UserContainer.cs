using Funfair.Dal.CosmosDb;
using Funfair.Shared.Core.Repository;
using Microsoft.Azure.Cosmos;

namespace Users.Infrastructure.DAL.Container;

public class UserContainer : ContainerContext, ICosmosUnitOfWork
{
    public UserContainer(Microsoft.Azure.Cosmos.Container container) : base(container)
    {
    }

    public TransactionalBatch CreateTransactionalBatch()
    {
        return Container.CreateTransactionalBatch(new PartitionKey());
    }

    public Task CommitAsync(TransactionalBatch batch, CancellationToken cancellationToken = default)
    {
        return batch.ExecuteAsync(cancellationToken);
    }
}