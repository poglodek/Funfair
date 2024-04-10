namespace Reservations.Core.ValueObjects;

public record Airport(string Name, string City, string IAtaCode);

public record Journey(Airport Departure, Airport Destination);