using System.Runtime.CompilerServices;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.ValueObjects;


namespace Reservations.Core.Entities;

public class UserReservation  : DomainBase
{
    public User User { get; private set; }
    public SeatId SeatId { get; private set; }
    public DateTime Purchased { get; private set; }
    public Price Price { get; private set; }

    private UserReservation() { }

    private UserReservation(Id id,User user, SeatId seat, DateTime purchased, Price price)
    {
        Id = id ?? Guid.NewGuid();
        User = user;
        SeatId = seat;
        Purchased = purchased;
        Price = price;
    }

    public static UserReservation Create(Id id, User user, SeatId seatId, DateTime purchased, Price price)
    {
        var flightReservation = new UserReservation(id, user, seatId, purchased, price);
        
        flightReservation.RaiseEvent(new NewUserReservationCreatedEvent(id,user.Id,seatId.Id));

        return flightReservation;
    }
}