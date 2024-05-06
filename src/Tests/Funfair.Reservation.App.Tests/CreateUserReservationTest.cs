using System.Collections.ObjectModel;
using Funfair.Shared.Core;
using Funfair.Shared.Core.Events;
using MediatR;
using NSubstitute;
using Reservations.App.Commands.CreateUserReservation;
using Reservations.App.Exceptions;
using Reservations.App.Services;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;
using Shouldly;
using Test.Shared;

namespace Funfair.Reservation.App.Tests;

public class CreateUserReservationTest
{
    private readonly IReservationRepository _reservationRepositoryMock = Substitute.For<IReservationRepository>();
    private readonly IEventDispatcher _eventDispatcher =  Substitute.For<IEventDispatcher>();
    private readonly IPlaneService _planeMock = Substitute.For<IPlaneService>();
    private readonly IClock _clock = new ClockTest("2022-10-21 12:12:12");
    
    [Fact]
    public async Task CreateUserReservation_AllValid_ShouldReturnUnitValue()
    {
        var reservation = GetValidReservation();
        var seat = GetValidSeat();
        _reservationRepositoryMock.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(reservation);
        _planeMock.GetSeatById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(seat);
        
        var command = new CreateUserReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        var handler = new CreateUserReservationCommandHandler(_reservationRepositoryMock, _planeMock, _clock, _eventDispatcher);

        var result = await handler.Handle(command, CancellationToken.None);
        
        
        result.ShouldBeOfType<Unit>();
    }
    
    [Fact]
    public async Task CreateUserReservation_AllValid_ShouldUpadteInRepository()
    {
        var reservation = GetValidReservation();
        var seat = GetValidSeat();
        _reservationRepositoryMock.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(reservation);
        _planeMock.GetSeatById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(seat);
        
        var command = new CreateUserReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        var handler = new CreateUserReservationCommandHandler(_reservationRepositoryMock, _planeMock, _clock, _eventDispatcher);

        var result = await handler.Handle(command, CancellationToken.None);
        
        await _reservationRepositoryMock.Received(1).Update(reservation, Arg.Any<CancellationToken>());
        
    }
    
    [Fact]
    public async Task CreateUserReservation_AllValid_ShouldPublishEventsFromReservation()
    {
        var reservation = GetValidReservation();
        var seat = GetValidSeat();
        _reservationRepositoryMock.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(reservation);
        _planeMock.GetSeatById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(seat);
        
        var command = new CreateUserReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        var handler = new CreateUserReservationCommandHandler(_reservationRepositoryMock, _planeMock, _clock, _eventDispatcher);

        var result = await handler.Handle(command, CancellationToken.None);
        
        await _eventDispatcher.Received(1).Publish(reservation, Arg.Any<CancellationToken>());

    }
    
    [Fact]
    public async Task CreateUserReservation_AllValid_ShouldPublishEventsFromUserReservation()
    {
        var reservation = GetValidReservation();
        var seat = GetValidSeat();
        _reservationRepositoryMock.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(reservation);
        _planeMock.GetSeatById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(seat);
        
        var command = new CreateUserReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        var handler = new CreateUserReservationCommandHandler(_reservationRepositoryMock, _planeMock, _clock, _eventDispatcher);

        var result = await handler.Handle(command, CancellationToken.None);

        await _eventDispatcher.Received(1).Publish(Arg.Any<UserReservation>(), Arg.Any<CancellationToken>());

    }
    
    [Fact]
    public async Task CreateUserReservation_ReservationNotExit_ShouldThrowAnException()
    {
        
        var seat = GetValidSeat();
        _planeMock.GetSeatById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(seat);
        
        var command = new CreateUserReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        var handler = new CreateUserReservationCommandHandler(_reservationRepositoryMock, _planeMock, _clock, _eventDispatcher);

        Record.ExceptionAsync(async ()=> await handler.Handle(command, CancellationToken.None)).Result.ShouldBeOfType<ReservationNotFound>();
  
    }
    
    [Fact]
    public async Task CreateUserReservation_SeatNotExit_ShouldThrowAnException()
    {
        
        var reservation = GetValidReservation();
        _reservationRepositoryMock.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(reservation);
        
        var command = new CreateUserReservationCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        var handler = new CreateUserReservationCommandHandler(_reservationRepositoryMock, _planeMock, _clock, _eventDispatcher);

        Record.ExceptionAsync(async ()=> await handler.Handle(command, CancellationToken.None)).Result.ShouldBeOfType<SeatNotFound>();
  
    }

    private Reservations.Core.Entities.Reservation GetValidReservation()
    {
        var draft = ReservationDraft.Create(Guid.NewGuid(), 
            new Journey(new Airport("Katowice","Katowice","KTW"),new Airport("New York","New York CITY","JFK")),
            new FlightDate(DateTime.Now, DateTime.Now),
            new Worker(Guid.NewGuid()), 
            new Plane(Guid.NewGuid(), new ReadOnlyCollection<Seat>(new List<Seat>{GetValidSeat()}))
            );
        
        return draft.Confirm(new Worker(Guid.NewGuid()), new Price(9825, "USD"), _clock);
    }

    private Seat GetValidSeat()
    {
        return new Seat(Guid.NewGuid(), "1", "A", ClassSeat.Economy);
    }
}