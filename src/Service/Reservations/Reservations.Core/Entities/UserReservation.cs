using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Reservations.Core.Events;
using Reservations.Core.Exceptions;
using Reservations.Core.Policy;
using Reservations.Core.ValueObjects;


namespace Reservations.Core.Entities;

public class UserReservation  : DomainBase
{
    public User User { get; private set; }
    public Seat Seat { get; private set; }
    public DateTimeOffset Purchased { get; private set; }
    public Price Price { get; private set; }

    private UserReservation() { }

    private UserReservation(Id id,User user, Seat seat, Price standardPrice, IClock clock)
    {
        Id = id ;
        User = user;
        Seat = seat;
        Purchased = clock.CurrentDateTime;
        Price = new PolicyPrice().CalculatePrice(seat, standardPrice);
    }
    
    public static UserReservation Create(Id id, User user, Seat seatId, Price price, IClock clock)
    {
        EnsureValidation(id, price);
        
        var flightReservation = new UserReservation(id, user, seatId, price, clock);
        
        flightReservation.RaiseEvent(new NewUserReservationCreatedEvent(id,user.Id,seatId.Id.Id));

        return flightReservation;
    }
    
    private static void EnsureValidation(Id id, Price price)
    {
        if (Guid.Empty == id.Value)
        {
            throw new InvalidReservationArgument("Id is not valid");
        }

        if (price.Value <= 1 || string.IsNullOrWhiteSpace(price.Currency))
        {
            throw new InvalidReservationArgument("Currency or price is invalid");
        }

        if (price.Currency.Trim().Length != 3)
        {
            throw new InvalidReservationArgument("Invalid currency type");
        }
    }

}