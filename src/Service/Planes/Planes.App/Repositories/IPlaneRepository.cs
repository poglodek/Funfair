using Planes.Core.Entities;

namespace Planes.App.Repositories;

public interface IPlaneRepository
{
    Task AddAsync(Plane plane);
    Task<Plane> GetAsync(Guid id);
    Task<Plane> GetByModelAsync(string model);
}