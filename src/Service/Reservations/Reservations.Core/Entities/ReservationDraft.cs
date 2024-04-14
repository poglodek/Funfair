using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.Exceptions;
using Reservations.Core.Specification;
using Reservations.Core.ValueObjects;

namespace Reservations.Core.Entities;

public class ReservationDraft : DomainBase
{
    public Journey Journey { get; private set; }
    public FlightDate FlightDate { get; private set; }
    public Worker CreatedBy { get; private set; }
    public Plane Plane { get; private set; }
    private Reservation? _reservation;
    
    private ReservationDraft() { }

    private ReservationDraft(Id id,Journey journey, FlightDate flightDate, Worker createdBy, Plane plane)
    {
        Id = id;
        Journey = journey;
        FlightDate = flightDate;
        CreatedBy = createdBy;
        Plane = plane;
    }
    
    
    public static ReservationDraft Create(Id id, Journey journey, FlightDate flightDate, Worker createdBy, Plane plane)
    {
        var reservationDraft = new ReservationDraft(id, journey, flightDate, createdBy, plane );
        
        reservationDraft.RaiseEvent(new NewReservationDraftCreatedEvent(id));

        return reservationDraft;
    }

    public void ChangeDeparture(Airport departure)
    {
        Journey = Journey with
        {
            Departure = departure
        };
        
    }

    public void ChangeDestiny(Airport destiny)
    {
        Journey = Journey with
        {
            Destination = destiny
        };
        
    }

    public void UpdatePlane(Plane plane)
    {
        Plane = plane;
    }

    public void UpdateDates(FlightDate flightDate)
    {
        FlightDate = flightDate;
    }

    public Reservation Confirm(Worker createdBy, IClock clock)
    {
        if (_reservation is not null)
        {
            throw new ReservationAlreadyExists(Id);
        }
        
        _reservation =  Reservation.Create(Guid.NewGuid(), Journey, FlightDate, createdBy, Plane, clock);
        
        if (!new SpecificationDue(clock).Check(_reservation))
        {
            throw new ReservationWillBeOverDue(Id);
        }


        return _reservation;
    }
}