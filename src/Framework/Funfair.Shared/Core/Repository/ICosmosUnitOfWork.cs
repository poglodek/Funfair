using Microsoft.Azure.Cosmos;

namespace Funfair.Shared.Core.Repository;

public interface ICosmosUnitOfWork
{
    TransactionalBatch CreateTransactionalBatch();
    Task CommitAsync(TransactionalBatch batch, CancellationToken cancellationToken = default);

}