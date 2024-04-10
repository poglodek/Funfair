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
    public Plane Plane { get; private set; }
    
    private ReservationDraft() { }

    private ReservationDraft(Id id,Journey journey, FlightDate flightDate, Worker createdBy, DateTimeOffset createdAt, Plane plane)
    {
        Id = id;
        Journey = journey;
        FlightDate = flightDate;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        Plane = plane;
        UpdatedAt = createdAt;
    }

    private void Update(IClock clock) => UpdatedAt = clock.CurrentDateTime;
    
    public static ReservationDraft Create(Id id, Journey journey, FlightDate flightDate, Worker createdBy,
        IClock clock, Plane plane)
    {
        var reservationDraft = new ReservationDraft(id, journey, flightDate, createdBy, clock.CurrentDateTime, plane );
        
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

    public void UpdatePlane(Plane plane, IClock clock)
    {
        Plane = plane;
        
        Update(clock);
    }

    public void UpdateDates(FlightDate flightDate, IClock clock)
    {
        FlightDate = flightDate;
        
        Update(clock);
    }

    public Reservation Confirm(Worker createdBy, IClock clock) => Reservation.Create(Guid.NewGuid(),Journey,FlightDate,createdBy,Plane, clock);
}