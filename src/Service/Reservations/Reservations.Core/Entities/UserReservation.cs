using Funfair.Shared.Core;
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

    private UserReservation(Id id,User user, SeatId seat, Price price, IClock clock)
    {
        Id = id ;
        User = user;
        SeatId = seat;
        Purchased = clock.CurrentDateTime;
        Price = price;
    }
    
    public static UserReservation Create(Id id, User user, SeatId seatId, Price price, IClock clock)
    {
        EnsureValidation(id, price);
        var flightReservation = new UserReservation(id, user, seatId, price, clock);
        
        flightReservation.RaiseEvent(new NewUserReservationCreatedEvent(id,user.Id,seatId.Id));

        return flightReservation;
    }
    
    private static void EnsureValidation(Id id, Price price)
    {
        if (Guid.Empty == id.Value)
        {
            throw new InvalidReservationArgument("Id is not valid");
        }

        if (price.Value <= 0 || string.IsNullOrWhiteSpace(price.Currency))
        {
            throw new InvalidReservationArgument("Currency or price is invalid");
        }

        if (price.Currency.Trim().Length != 3)
        {
            throw new InvalidReservationArgument("Invalid currency type");
        }
    }

}