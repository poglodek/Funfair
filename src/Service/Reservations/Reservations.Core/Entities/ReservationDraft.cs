using Funfair.Shared.Domain;
using Reservations.Core.ValueObjects;

namespace Reservations.Core.Entities;

public class ReservationDraft : DomainBase
{
    public Airport Airport { get; private set; }
    public FlightDate FlightDate { get; private set; }
    public Worker CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
}