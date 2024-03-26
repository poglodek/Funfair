using System.Runtime.CompilerServices;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.ValueObjects;


namespace Reservations.Core.Entities;

public class UserReservation  : DomainBase
{
    public User User { get; private set; }
    public Seat Seat { get; private set; }
    public DateTime Purchased { get; private set; }
    public Price Price { get; private set; }

    private UserReservation() { }

    private UserReservation(Id id,User user, Seat seat, DateTime purchased, Price price)
    {
        Id = id;
        User = user;
        Seat = seat;
        Purchased = purchased;
        Price = price;
    }

    public static UserReservation Create(Id id, User user, Seat seat, DateTime purchased, Price price)
    {
        var flightReservation = new UserReservation(id, user, seat, purchased, price);
        
        flightReservation.RaiseEvent(new NewUserReservationEvent(id,user.Id,seat.Id));

        return flightReservation;
    }
}