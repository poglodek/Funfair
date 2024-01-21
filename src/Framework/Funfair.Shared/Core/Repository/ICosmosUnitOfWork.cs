using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Funfair.Shared.Core.Repository;

public interface ICosmosUnitOfWork
{
    TransactionalBatch CreateTransactionalBatch();
    Task CommitAsync(TransactionalBatch batch, CancellationToken cancellationToken = default);

}