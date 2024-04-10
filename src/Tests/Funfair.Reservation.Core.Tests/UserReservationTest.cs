using Funfair.Shared.Domain;
using Reservations.Core.Entities;
using Reservations.Core.Events;
using Reservations.Core.Exceptions;
using Reservations.Core.ValueObjects;
using Shouldly;
using Test.Shared;
using Xunit;

namespace Funfair.Reservation.Core.Tests;

public class UserReservationTest
{
    [Fact]
    public void CreateUserReservation_AllValid_ShouldReturn()
    {
        var id = Guid.NewGuid();
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();
        
        var userReservation = UserReservation.Create(id, user, seatId
            , new Price(12.3, "USD"), clock);

        userReservation.ShouldNotBeNull();
        userReservation.Price.Currency.ShouldBe("USD");
        userReservation.Price.Value.ShouldBe(12.3);
        userReservation.Id.Value.ShouldBe(id);
        userReservation.User.Id.ShouldBe(user.Id);
        userReservation.SeatId.Id.ShouldBe(seatId.Id);
        userReservation.Purchased.ShouldBe(clock.CurrentDateTime);
    }

    [Fact]
    public void CreateUserReservation_IdInvalid_ShouldThrowAnException()
    {
        var id = Guid.Empty;
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();

        var ex = Record.Exception(() => UserReservation.Create(id, user, seatId
            , new Price(12.3, "USD"), clock));

        ex.ShouldBeOfType<InvalidReservationArgument>();
    }
    
    [Fact]
    public void CreateUserReservation_0Price_ShouldThrowAnException()
    {
        var id = Guid.NewGuid();
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();

        var ex = Record.Exception(() => UserReservation.Create(id, user, seatId
            , new Price(0, "USD"), clock));

        ex.ShouldBeOfType<InvalidReservationArgument>();
    }
    
    [Fact]
    public void CreateUserReservation_NegativePrice_ShouldThrowAnException()
    {
        var id = Guid.NewGuid();
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();

        var ex = Record.Exception(() => UserReservation.Create(id, user, seatId
            , new Price(-150.75, "USD"), clock));

        ex.ShouldBeOfType<InvalidReservationArgument>();
    }
    
    [Fact]
    public void CreateUserReservation_CurrencyInValid_ShouldThrowAnException()
    {
        var id = Guid.NewGuid();
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();

        var ex = Record.Exception(() => UserReservation.Create(id, user, seatId
            , new Price(42.0, ""), clock));

        ex.ShouldBeOfType<InvalidReservationArgument>();
    }
    
        
    [Fact]
    public void CreateUserReservation_CurrencyNull_ShouldThrowAnException()
    {
        var id = Guid.NewGuid();
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();

        var ex = Record.Exception(() => UserReservation.Create(id, user, seatId
            , new Price(42.0, null), clock));

        ex.ShouldBeOfType<InvalidReservationArgument>();
    }

    
        
    [Fact]
    public void CreateUserReservation_CurrencyWhiteChars_ShouldThrowAnException()
    {
        var id = Guid.NewGuid();
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();

        var ex = Record.Exception(() => UserReservation.Create(id, user, seatId
            , new Price(42.0, "    "), clock));

        ex.ShouldBeOfType<InvalidReservationArgument>();
    }
    
         
    [Fact]
    public void CreateUserReservation_CurrencyInvalidType_ShouldThrowAnException()
    {
        var id = Guid.NewGuid();
        var user = GetValidUser;
        var seatId = GetValidSeatId;
        var clock = new ClockTest();

        var ex = Record.Exception(() => UserReservation.Create(id, user, seatId
            , new Price(42.0, "CURRY"), clock));

        ex.ShouldBeOfType<InvalidReservationArgument>();
    }

    
    private static User GetValidUser => new User(Guid.NewGuid());
    private static SeatId GetValidSeatId => new(Guid.NewGuid());
}