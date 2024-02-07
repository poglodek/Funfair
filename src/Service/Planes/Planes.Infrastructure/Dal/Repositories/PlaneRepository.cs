using Funfair.Dal.CosmosDb.Linq;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Planes.App.Repositories;
using Planes.Core.Entities;
using Planes.Infrastructure.Dal.Container;

namespace Planes.Infrastructure.Dal.Repositories;

internal class PlaneRepository(PlaneContainer container) : IPlaneRepository
{
    private readonly Microsoft.Azure.Cosmos.Container _container = container.Container;

    public Task AddAsync(Plane plane, CancellationToken token)
    {
        return _container.CreateItemAsync(plane, cancellationToken: token);
    }

    public Task<Plane?> GetAsync(Guid id, CancellationToken token)
    {
        return _container.GetItemLinqQueryable<Plane>()
            .Where(x => x.Id.Value == id)
            .ToFeedIterator()
            .FirstOrDefaultAsync(token);

    }

    public Task<Plane?> GetByModelAsync(string model, CancellationToken token)
    {
        return _container.GetItemLinqQueryable<Plane>()
            .Where(x => x.Model.Value == model)
            .ToFeedIterator()
            .FirstOrDefaultAsync(token);
    }

    public Task<List<Plane>> GetAll(CancellationToken cancellationToken)
    {
        return _container.GetItemLinqQueryable<Plane>()
            .ToFeedIterator()
            .ToListAsync(cancellationToken);
    }
}