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
    private readonly IClock _mockClock = new ClockTest("2022-01-01T00:00:00Z");
    private readonly Price _price;

    public ReservationTests()
    {
        _id = new Id(Guid.NewGuid());
        var departure = new Airport("Departure","Warsaw","WWA");
        var destination = new Airport("Destination","New York","JFK");
        _journey = new Journey(departure, destination);
        _flightDate = new FlightDate(_mockClock.CurrentDateTime.AddDays(12), _mockClock.CurrentDateTime.AddDays(12).AddHours(4));
        _createdBy = new Worker(Guid.NewGuid());
        _plane = new Plane(Guid.NewGuid(), new ReadOnlyCollection<Seat>(new List<Seat>()));
        _price = new Price(100, "USD");
    }
    
    [Fact]
    public void CreateReservation_AllValid_ShouldReturn()
    {
        //fix this
        var draft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _plane);
        var reservation = Reservations.Core.Entities.Reservation.Create(draft, _price);

        reservation.ShouldNotBeNull();
        reservation.Id.ShouldBe(_id);
        reservation.Journey.ShouldBe(_journey);
        reservation.FlightDate.ShouldBe(_flightDate);
        reservation.CreatedBy.ShouldBe(_createdBy);
        reservation.Plane.ShouldBe(_plane);
        reservation.DomainEvents.Count.ShouldBe(1);
    }
    
    [Fact]
    public void AddUserReservation_AllValid_ShouldReturn()
    {
        var draft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _plane);
        var reservation = Reservations.Core.Entities.Reservation.Create(draft, _price);
        var user = new User(Guid.NewGuid());
        var seatId = new Seat(Guid.NewGuid(), "1","A", ClassSeat.Economy);
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
        var draft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _plane);
        var reservation = Reservations.Core.Entities.Reservation.Create(draft, _price);
        var user = new User(Guid.NewGuid());
        var seatId = new Seat(Guid.NewGuid(), "1","A", ClassSeat.Economy);
        var userReservation = UserReservation.Create(Guid.NewGuid(), user, seatId, new Price(12.3, "USD"), _mockClock);

        reservation.AddUserReservation(userReservation, _mockClock);

        var ex = Record.Exception(() => reservation.AddUserReservation(userReservation, _mockClock));

        ex.ShouldBeOfType<UserReservationAlreadyExists>();
    }
    
    [Fact]
    public void CancelUserReservation_AllValid_ShouldReturn()
    {
        var draft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _plane);
        var reservation = Reservations.Core.Entities.Reservation.Create(draft, _price);
        var user = new User(Guid.NewGuid());
        var seatId = new Seat(Guid.NewGuid(), "1","A", ClassSeat.Economy);
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
        var draft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _plane);
        var reservation = Reservations.Core.Entities.Reservation.Create(draft, _price);
        var user = new User(Guid.NewGuid());
        var seatId = new Seat(Guid.NewGuid(), "1","A", ClassSeat.Economy);
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
        var draft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _plane);
        var reservation = Reservations.Core.Entities.Reservation.Create(draft, _price);
        var user = new User(Guid.NewGuid());
        var seatId = new Seat(Guid.NewGuid(), "1","A", ClassSeat.Economy);
        var userReservation = UserReservation.Create(Guid.NewGuid(), user, seatId, new Price(12.3, "USD"), _mockClock);

        var ex = Record.Exception(() => reservation.CancelUserReservation(userReservation, _mockClock));

        ex.ShouldBeOfType<UserReservationDontExists>();
    }
    
    
}