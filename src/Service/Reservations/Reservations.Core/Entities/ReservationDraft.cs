using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.ValueObjects;

namespace Reservations.Core.Entities;

public class ReservationDraft : DomainBase
{
    public Journey Journey { get; private set; }
    public FlightDate FlightDate { get; private set; }
    public Worker CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    
    private ReservationDraft() { }

    private ReservationDraft(Id id,Journey journey, FlightDate flightDate, Worker createdBy, DateTimeOffset createdAt)
    {
        Id = id;
        Journey = journey;
        FlightDate = flightDate;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private void Update(IClock clock) => UpdatedAt = clock.CurrentDateTime;
    
    public static ReservationDraft Create(Id id, Journey journey, FlightDate flightDate, Worker createdBy,
        DateTime createdAt)
    {
        var reservationDraft = new ReservationDraft(id, journey, flightDate, createdBy, createdAt);
        
        reservationDraft.RaiseEvent(new NewReservationDraftCreatedEvent(id));

        return reservationDraft;
    }

    public void ChangeDeparture(Airport departure, IClock clock)
    {
        Journey = Journey with
        {
            Departure = departure
        };
        
        Update(clock);
    }

    public void ChangeDestiny(Airport destiny, IClock clock)
    {
        Journey = Journey with
        {
            Destination = destiny
        };
        
        Update(clock);
    }
}