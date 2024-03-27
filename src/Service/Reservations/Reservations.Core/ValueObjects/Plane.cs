using System.Collections.ObjectModel;

namespace Reservations.Core.ValueObjects;

public record Plane(Guid Id, ReadOnlyCollection<Seat> Seats);