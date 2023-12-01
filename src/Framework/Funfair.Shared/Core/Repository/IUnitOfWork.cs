using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Funfair.Shared.Core.Repository;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}