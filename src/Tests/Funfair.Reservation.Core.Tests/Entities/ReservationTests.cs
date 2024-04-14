using System.Collections.ObjectModel;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Reservations.Core.Entities;
using Reservations.Core.Exceptions;
using Reservations.Core.ValueObjects;
using Shouldly;
using Test.Shared;
using Xunit;

namespace Funfair.Reservation.Core.Tests.Entities;

public class ReservationTests
{
    private readonly Id _id;
    private readonly Journey _journey;
    private readonly FlightDate _flightDate;
    private readonly Worker _createdBy;
    private readonly Plane _plane;
    private readonly IClock _mockClock = new ClockTest();

    public ReservationTests()
    {
        _id = new Id(Guid.NewGuid());
        var departure = new Airport("Departure","Warsaw","WWA");
        var destination = new Airport("Destination","New York","JFK");
        _journey = new Journey(departure, destination);
        _flightDate = new FlightDate(_mockClock.CurrentDateTime.AddDays(12), _mockClock.CurrentDateTime.AddDays(12).AddHours(4));
        _createdBy = new Worker(Guid.NewGuid());
        _plane = new Plane(Guid.NewGuid(), new ReadOnlyCollection<Seat>(new List<Seat>()));
    }
    
    [Fact]
    public void CreateReservation_AllValid_ShouldReturn()
    {
        var reservation = Reservations.Core.Entities.Reservation.Create(_id, _journey, _flightDate, _createdBy, _plane,_mockClock);

        reservation.ShouldNotBeNull();
        reservation.Id.ShouldBe(_id);
        reservation.Journey.ShouldBe(_journey);
        reservation.FlightDate.ShouldBe(_flightDate);
        reservation.CreatedBy.ShouldBe(_createdBy);
        reservation.CreatedAt.ShouldBe(_mockClock.CurrentDateTime);
        reservation.Plane.ShouldBe(_plane);
        reservation.DomainEvents.Count.ShouldBe(1);
    }
    
    [Fact]
    public void AddUserReservation_AllValid_ShouldReturn()
    {
        var reservation = Reservations.Core.Entities.Reservation.Create(_id, _journey, _flightDate, _createdBy, _plane,_mockClock);
        var user = new User(Guid.NewGuid());
        var seatId = new SeatId(Guid.NewGuid());
        var userReservation = UserReservation.Create(Guid.NewGuid(), user, seatId, new Price(12.3, "USD"), _mockClock);

        ((IDomainBase)reservation).ClearEvents();
        
        reservation.AddUserReservation(userReservation, _mockClock);

        reservation.UserReservations.Count.ShouldBe(1);
        reservation.UserReservations.First().ShouldBe(userReservation);
        reservation.DomainEvents.Count.ShouldBe(1);
    }
    
    [Fact]
    public void AddUserReservation_UserReservationAlreadyExists_ShouldThrowAnException()
    {
        var reservation = Reservations.Core.Entities.Reservation.Create(_id, _journey, _flightDate, _createdBy, _plane,_mockClock);
        var user = new User(Guid.NewGuid());
        var seatId = new SeatId(Guid.NewGuid());
        var userReservation = UserReservation.Create(Guid.NewGuid(), user, seatId, new Price(12.3, "USD"), _mockClock);

        reservation.AddUserReservation(userReservation, _mockClock);

        var ex = Record.Exception(() => reservation.AddUserReservation(userReservation, _mockClock));

        ex.ShouldBeOfType<UserReservationAlreadyExists>();
    }
    
    [Fact]
    public void CancelUserReservation_AllValid_ShouldReturn()
    {
        var reservation = Reservations.Core.Entities.Reservation.Create(_id, _journey, _flightDate, _createdBy, _plane,_mockClock);
        var user = new User(Guid.NewGuid());
        var seatId = new SeatId(Guid.NewGuid());
        var userReservation = UserReservation.Create(Guid.NewGuid(), user, seatId, new Price(12.3, "USD"), _mockClock);

        reservation.AddUserReservation(userReservation, _mockClock);
        ((IDomainBase)reservation).ClearEvents();
        
        reservation.CancelUserReservation(userReservation, _mockClock);

        reservation.UserReservations.Count.ShouldBe(0);
        reservation.DomainEvents.Count.ShouldBe(1);
    }
    
    [Fact]
    public void CancelUserReservation_ReservationCannotBeEdited_ShouldThrowAnException()
    {
        var reservation = Reservations.Core.Entities.Reservation.Create(_id, _journey, _flightDate, _createdBy, _plane,_mockClock);
        var user = new User(Guid.NewGuid());
        var seatId = new SeatId(Guid.NewGuid());
        var userReservation = UserReservation.Create(Guid.NewGuid(), user, seatId, new Price(12.3, "USD"), _mockClock);

        reservation.AddUserReservation(userReservation, _mockClock);
        ((IDomainBase)reservation).ClearEvents();
        reservation.CancelUserReservation(userReservation, _mockClock);      

        var ex = Record.Exception(() => reservation.CancelUserReservation(userReservation, _mockClock));

        ex.ShouldBeOfType<UserReservationDontExists>();
    }
    
    [Fact]
    public void CancelUserReservation_UserReservationDoesntExists_ShouldThrowAnException()
    {
        var reservation = Reservations.Core.Entities.Reservation.Create(_id, _journey, _flightDate, _createdBy, _plane,_mockClock);
        var user = new User(Guid.NewGuid());
        var seatId = new SeatId(Guid.NewGuid());
        var userReservation = UserReservation.Create(Guid.NewGuid(), user, seatId, new Price(12.3, "USD"), _mockClock);

        var ex = Record.Exception(() => reservation.CancelUserReservation(userReservation, _mockClock));

        ex.ShouldBeOfType<UserReservationDontExists>();
    }
    
    
}