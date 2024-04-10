
using Xunit;
using System.Collections.ObjectModel;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Reservations.Core.Entities;
using Reservations.Core.Exceptions;
using Reservations.Core.ValueObjects;
using Test.Shared;
using Assert = Xunit.Assert;

public class ReservationDraftTests
{
    private readonly IClock _mockClock = new ClockTest();
    private readonly Id _id;
    private readonly Journey _journey;
    private readonly FlightDate _flightDate;
    private readonly Worker _createdBy;
    private readonly Plane _plane;

    public ReservationDraftTests()
    {
        _id = new Id(Guid.NewGuid());
        _journey = new Journey(new Airport("Departure","Warsaw","WWA"), new Airport("Destination","New York","JFK"));
        _flightDate = new FlightDate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2));
        _createdBy = new Worker(Guid.NewGuid());
        _plane = new Plane(Guid.NewGuid(), new ReadOnlyCollection<Seat>(new List<Seat>()));
    }

    [Fact]
    public void Create_ShouldCreateReservationDraft()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _mockClock, _plane);

        Assert.NotNull(reservationDraft);
    }

    [Fact]
    public void ChangeDeparture_ShouldUpdateDeparture()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _mockClock, _plane);
        var newDeparture = new Airport("New Departure", "Los angeles", "LAX");

        reservationDraft.ChangeDeparture(newDeparture, _mockClock);

        Assert.Equal(newDeparture, reservationDraft.Journey.Departure);
    }

    [Fact]
    public void ChangeDestiny_ShouldUpdateDestination()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _mockClock, _plane);
        var newDestination = new Airport("New Departure", "Los angeles", "LAX");

        reservationDraft.ChangeDestiny(newDestination, _mockClock);

        Assert.Equal(newDestination, reservationDraft.Journey.Destination);
    }

    [Fact]
    public void UpdatePlane_ShouldUpdatePlane()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _mockClock, _plane);
        var newPlane = new Plane(Guid.NewGuid(), new ReadOnlyCollection<Seat>(new List<Seat>()));

        reservationDraft.UpdatePlane(newPlane, _mockClock);

        Assert.Equal(newPlane, reservationDraft.Plane);
    }

    [Fact]
    public void UpdateDates_ShouldUpdateFlightDate()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _mockClock, _plane);
        var newFlightDate = new FlightDate(DateTimeOffset.UtcNow.AddHours(1), DateTimeOffset.UtcNow.AddHours(3));

        reservationDraft.UpdateDates(newFlightDate, _mockClock);

        Assert.Equal(newFlightDate, reservationDraft.FlightDate);
    }

    [Fact]
    public void Confirm_ShouldCreateReservation()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _mockClock, _plane);

        var reservation = reservationDraft.Confirm(_createdBy, _mockClock);

        Assert.NotNull(reservation);
        //Asserts for reservation properties
        Assert.Equal(reservationDraft.Plane, reservation.Plane);
        Assert.Equal(reservationDraft.Journey, reservation.Journey);
        Assert.Equal(reservationDraft.FlightDate, reservation.FlightDate);
        Assert.Equal(reservationDraft.CreatedBy, _createdBy);
        ;
    }

    [Fact]
    public void Confirm_ShouldThrowException_WhenReservationAlreadyExists()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy, _mockClock, _plane);
        reservationDraft.Confirm(_createdBy, _mockClock);

        Assert.Throws<ReservationAlreadyExists>(() => reservationDraft.Confirm(_createdBy, _mockClock));
    }
}
