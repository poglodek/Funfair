using System.Collections.ObjectModel;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.Exceptions;
using Reservations.Core.ValueObjects;

namespace Reservations.Core.Entities;

public class Reservation : DomainBase
{
    private readonly HashSet<UserReservation> _userReservations = new();

    public ReadOnlyCollection<UserReservation> UserReservations
        => _userReservations.ToList().AsReadOnly();

    public Airport Airport { get; private set; }
    public FlightDate FlightDate { get; private set; }
    public Worker CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Reservation() { }

    private Reservation(Id id,Airport airport, FlightDate flightDate, Worker createdBy, DateTime createdAt)
    {
        Id = id;
        Airport = airport;
        FlightDate = flightDate;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }

    public static Reservation Create(Id id, Airport airport, FlightDate flightDate, Worker createdBy,
        DateTime createdAt)
    {
        var reservation = new Reservation(id, airport, flightDate, createdBy, createdAt);
        
        reservation.RaiseEvent(new NewReservationCreatedEvent(id,airport,flightDate,createdBy));

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
    }

    public void CancelReservation(UserReservation userReservation)
    {
        var reservation = _userReservations.FirstOrDefault(x => x.Id == userReservation.Id);
        if (reservation is null)
        {
            throw new UserReservationDontExists($"User reservation don't exists for reservation Id: {Id.Value} ");
        }

        _userReservations.RemoveWhere(x => x.Id == userReservation.Id);
    }
}