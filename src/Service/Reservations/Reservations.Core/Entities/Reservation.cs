using System.Collections.ObjectModel;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.Exceptions;
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

    private Reservation(Id id,Journey journey, FlightDate flightDate, Worker createdBy, DateTimeOffset createdAt)
    {
        Id = id;
        Journey = journey;
        FlightDate = flightDate;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }

    public static Reservation Create(Id id, Journey journey, FlightDate flightDate, Worker createdBy, Plane plane,
        IClock clock)
    {
        var reservation = new Reservation(id, journey, flightDate, createdBy, clock.CurrentDateTime);
        
        reservation.RaiseEvent(new NewReservationCreatedEvent(id));

        return reservation;
    }

    public void AddUserReservation(UserReservation userReservation)
    {
        if (_userReservations.Any(x => x.Id == userReservation.Id)
            || _userReservations.Any(x => x.User.Id == userReservation.User.Id))
        {
            throw new UserReservationAlreadyExists(
                $"reservation for id {Id} already exists for user {userReservation.User.Id}");
        }

        _userReservations.Add(userReservation);
        
        RaiseEvent(new UserReservationAssignedReservationEvents(Id,userReservation.Id));
    }

    public void CancelUserReservation(UserReservation userReservation)
    {
        var reservation = _userReservations.FirstOrDefault(x => x.Id == userReservation.Id);
        if (reservation is null)
        {
            throw new UserReservationDontExists($"User reservation don't exists for reservation Id: {Id.Value} ");
        }

        _userReservations.RemoveWhere(x => x.Id == userReservation.Id);
        
        RaiseEvent(new UserReservationRemovedFromReservationEvents(Id,userReservation.Id));
    }
}