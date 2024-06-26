using System.Collections.ObjectModel;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.Exceptions;
using Reservations.Core.Specification;
using Reservations.Core.ValueObjects;

namespace Reservations.Core.Entities;

public class Reservation : AggregateRoot
{
    private readonly HashSet<UserReservation> _userReservations = new();

    public ReadOnlyCollection<UserReservation> UserReservations
        => _userReservations.ToList().AsReadOnly();

    public Journey Journey { get; init; }
    public FlightDate FlightDate { get; init; }
    public Worker CreatedBy { get; init; }
    public Plane Plane { get; init; }
    public Price StandardPrice { get; init; }


    private Reservation() { }

    private Reservation(Id id,Journey journey, FlightDate flightDate, Worker createdBy, Plane plane, Price standardPrice)
    {
        Id = id ?? throw new ArgumentCoreNullException(nameof(id));
        Journey = journey ?? throw new ArgumentCoreNullException(nameof(journey));
        FlightDate = flightDate ?? throw new ArgumentCoreNullException(nameof(flightDate));
        CreatedBy = createdBy ?? throw new ArgumentCoreNullException(nameof(createdBy));
        Plane = plane ?? throw new ArgumentCoreNullException(nameof(plane));
        StandardPrice = standardPrice ?? throw new ArgumentCoreNullException(nameof(standardPrice));
    }

    internal static Reservation Create(ReservationDraft draft, Price price)
    {
        var reservation = new Reservation(draft.Id, draft.Journey, draft.FlightDate, draft.CreatedBy, draft.Plane, price);
        
        reservation.RaiseEvent(new NewReservationCreatedEvent(reservation.Id));

        return reservation;
    }

    public void AddUserReservation(UserReservation userReservation, IClock clock)
    {
        if (!new SpecificationDue(clock).Check(this))
        {
            throw new UserReservationCantBeEdited($"User reservation can't be added to reservation {Id.Value} because the flight date is in the past");
        }
        
        if (_userReservations.Any(x => x.Id == userReservation.Id)
            || _userReservations.Any(x => x.User.Id == userReservation.User.Id))
        {
            throw new UserReservationAlreadyExists(
                $"reservation for id {Id} already exists for user {userReservation.User.Id}");
        }
        
        if(_userReservations.Any(x=>x.Seat.Id == userReservation.Seat.Id))
        {
            throw new UserReservationSeatAlreadyExists(
                $"reservation for id {Id} already exists for seat {userReservation.Seat.Id}");
        }

        _userReservations.Add(userReservation);
        
        RaiseEvent(new UserReservationAssignedReservationEvents(Id,userReservation.Id));
    }

    public void CancelUserReservation(UserReservation userReservation, IClock clock)
    {
        if (!new SpecificationDue(clock).Check(this))
        {
            throw new UserReservationCantBeEdited($"User reservation can't be added to reservation {Id.Value} because the flight date is in the past");
        }
        
        var reservation = _userReservations.FirstOrDefault(x => x.Id == userReservation.Id);
        if (reservation is null)
        {
            throw new UserReservationDontExists($"User reservation don't exists for reservation Id: {Id.Value} ");
        }

        _userReservations.RemoveWhere(x => x.Id == userReservation.Id);
        
        RaiseEvent(new UserReservationRemovedFromReservationEvents(Id,userReservation.Id));
    }
    

}