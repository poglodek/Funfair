using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Funfair.Shared.Core.Repository;

public interface IChangeTrack
{
    ChangeTracker ChangeTracker { get; }
}