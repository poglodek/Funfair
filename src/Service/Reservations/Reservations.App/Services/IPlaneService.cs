using Reservations.Core.ValueObjects;

namespace Reservations.App.Services;

public interface IPlaneService
{
    Task<Plane> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Seat> GetSeatById(Guid requestSeatId, CancellationToken cancellationToken);
}