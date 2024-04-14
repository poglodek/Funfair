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

    public Journey Journey { get; private set; }
    public FlightDate FlightDate { get; init; }
    public Worker CreatedBy { get; init; }
    public Plane Plane { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    
    
    private Reservation() { }

    private Reservation(Id id,Journey journey, FlightDate flightDate, Worker createdBy, DateTimeOffset createdAt, Plane plane)
    {
        Id = id ?? throw new ArgumentCoreNullException(nameof(id));
        Journey = journey ?? throw new ArgumentCoreNullException(nameof(journey));
        FlightDate = flightDate ?? throw new ArgumentCoreNullException(nameof(flightDate));
        CreatedBy = createdBy ?? throw new ArgumentCoreNullException(nameof(createdBy));
        CreatedAt = createdAt;
        Plane = plane ?? throw new ArgumentCoreNullException(nameof(plane));
    }

    public static Reservation Create(Id id, Journey journey, FlightDate flightDate, Worker createdBy, Plane plane,
        IClock clock)
    {
        var reservation = new Reservation(id, journey, flightDate, createdBy, clock.CurrentDateTime, plane);
        
        reservation.RaiseEvent(new NewReservationCreatedEvent(id));

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