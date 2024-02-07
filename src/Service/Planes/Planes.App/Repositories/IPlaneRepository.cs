using Planes.Core.Entities;

namespace Planes.App.Repositories;

public interface IPlaneRepository
{
    Task AddAsync(Plane plane, CancellationToken token);
    Task<Plane?> GetAsync(Guid id, CancellationToken token);
    Task<Plane?> GetByModelAsync(string model, CancellationToken token);
    Task<List<Plane>>GetAll(CancellationToken cancellationToken);
}