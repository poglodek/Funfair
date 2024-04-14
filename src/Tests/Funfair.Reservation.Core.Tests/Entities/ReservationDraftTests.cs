
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
    private readonly IClock _mockClock = new ClockTest("2022-01-01T00:00:00Z");
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
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy,  _plane);

        Assert.NotNull(reservationDraft);
    }

    [Fact]
    public void ChangeDeparture_ShouldUpdateDeparture()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy,  _plane);
        var newDeparture = new Airport("New Departure", "Los angeles", "LAX");

        reservationDraft.ChangeDeparture(newDeparture);

        Assert.Equal(newDeparture, reservationDraft.Journey.Departure);
    }

    [Fact]
    public void ChangeDestiny_ShouldUpdateDestination()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy,  _plane);
        var newDestination = new Airport("New Departure", "Los angeles", "LAX");

        reservationDraft.ChangeDestiny(newDestination);

        Assert.Equal(newDestination, reservationDraft.Journey.Destination);
    }

    [Fact]
    public void UpdatePlane_ShouldUpdatePlane()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy,  _plane);
        var newPlane = new Plane(Guid.NewGuid(), new ReadOnlyCollection<Seat>(new List<Seat>()));

        reservationDraft.UpdatePlane(newPlane);

        Assert.Equal(newPlane, reservationDraft.Plane);
    }

    [Fact]
    public void UpdateDates_ShouldUpdateFlightDate()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy,  _plane);
        var newFlightDate = new FlightDate(DateTimeOffset.UtcNow.AddHours(1), DateTimeOffset.UtcNow.AddHours(3));

        reservationDraft.UpdateDates(newFlightDate);

        Assert.Equal(newFlightDate, reservationDraft.FlightDate);
    }

    [Fact]
    public void Confirm_ShouldCreateReservation()
    {
        var reservationDraft = ReservationDraft.Create(_id, _journey, _flightDate, _createdBy,  _plane);

        var reservation = reservationDraft.Confirm(_createdBy, _mockClock);

        Assert.NotNull(reservation);
        //Asserts for reservation properties
        Assert.Equal(reservationDraft.Plane, reservation.Plane);
        Assert.Equal(reservationDraft.Journey, reservation.Journey);
        Assert.Equal(reservationDraft.FlightDate, reservation.FlightDate);
        Assert.Equal(reservationDraft.CreatedBy, _createdBy);
        ;
    }

}
