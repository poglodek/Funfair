using System.Collections.ObjectModel;
using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using MediatR;
using NSubstitute;
using Reservations.App.Commands.CreateDraft;
using Reservations.App.Commands.UpdateDraft;
using Reservations.App.Exceptions;
using Reservations.App.Services;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;
using Shouldly;
using Test.Shared;

namespace Funfair.Reservation.App.Tests;

public class UpdateDraftTest
{
    private readonly IReservationRepository _reservationRepositoryMock = Substitute.For<IReservationRepository>();
    private readonly IEventProcessor _mockEventProcessor =  Substitute.For<IEventProcessor>();
    private readonly IPlaneService _planeMock = Substitute.For<IPlaneService>();
    private readonly IClock _clock = new ClockTest("2022-10-21 12:12:12");

    [Fact]
    public async Task UpdateDateDraft_AllValid_ShouldReturnUnitValue()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        
        var clock = new ClockTest("2024-05-05 00:00:00");
        var flightDate = new FlightDate(clock.CurrentDateTime, clock.CurrentDateTime.AddHours(8));
        var command = new UpdateDateDraftCommand(Guid.NewGuid(), flightDate);
        
        var handler = new UpdateDateDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, GetAccessor());

        var result = await handler.Handle(command, CancellationToken.None);
        
        await _reservationRepositoryMock.Received(1).Update(draft, Arg.Any<CancellationToken>());
        await _mockEventProcessor.Received(1).ProcessAsync(draft, Arg.Any<CancellationToken>());
        
        draft.FlightDate.ShouldBe(flightDate);
        result.ShouldBeOfType<Unit>();
    }
    
    [Fact]
    public async Task UpdateDateDraft_InvalidClaims_ShouldThrowAnException()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);

        var clock = new ClockTest("2024-05-05 00:00:00");
        var flightDate = new FlightDate(clock.CurrentDateTime, clock.CurrentDateTime.AddHours(8));
        var command = new UpdateDateDraftCommand(Guid.NewGuid(), flightDate);
        
        var claims = new Dictionary<string, string>{{"User","User"}};
        var accessor = GetAccessor(claims:claims);
        
        var handler = new UpdateDateDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, accessor);

        await Should.ThrowAsync<UnauthorizedAccessException>(async () => await handler.Handle(command, CancellationToken.None));
    }
    
    
    [Fact]
    public async Task UpdateDepartureDraft_AllValid_ShouldReturnUnit()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        var airport = new Airport("New York CIty Airport", "New York", "JFK");
        var command = new UpdateDepartureDraftCommand(Guid.NewGuid(), airport);
        
        var handler = new UpdateDepartureDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, GetAccessor());

        var result = await handler.Handle(command, CancellationToken.None);
        
        await _reservationRepositoryMock.Received(1).Update(draft, Arg.Any<CancellationToken>());
        await _mockEventProcessor.Received(1).ProcessAsync(draft, Arg.Any<CancellationToken>());
        
        draft.Journey.Departure.ShouldBe(airport);
        result.ShouldBeOfType<Unit>();
    }
    
    [Fact]
    public async Task UpdateDepartureDraft_InvalidClaims_ShouldReturnUnit()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        var airport = new Airport("New York CIty Airport", "New York", "JFK");
        var command = new UpdateDepartureDraftCommand(Guid.NewGuid(), airport);
        
        var claims = new Dictionary<string, string>{{"User","User"}};
        var accessor = GetAccessor(claims:claims);
        
        var handler = new UpdateDepartureDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, accessor);

        await Should.ThrowAsync<UnauthorizedAccessException>(async () => await handler.Handle(command, CancellationToken.None));
 
    }


    [Fact]
    public async Task UpdateDestinyDraft_AllValid_ShouldReturnUnit()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        var airport = new Airport("New York CIty Airport", "New York", "JFK");
        var command = new UpdateDestinyDraftCommand(Guid.NewGuid(), airport);
        
        var handler = new UpdateDestinyDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, GetAccessor());

        var result = await handler.Handle(command, CancellationToken.None);
        
        await _reservationRepositoryMock.Received(1).Update(draft, Arg.Any<CancellationToken>());
        await _mockEventProcessor.Received(1).ProcessAsync(draft, Arg.Any<CancellationToken>());
        
        draft.Journey.Departure.ShouldBe(airport);
        result.ShouldBeOfType<Unit>();
    }
    
    [Fact]
    public async Task UpdateDestinyDraft_InvalidClaims_ShouldReturnUnit()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        var airport = new Airport("New York CIty Airport", "New York", "JFK");
        var command = new UpdateDestinyDraftCommand(Guid.NewGuid(), airport);
        
        var claims = new Dictionary<string, string>{{"User","User"}};
        var accessor = GetAccessor(claims:claims);
        
        var handler = new UpdateDestinyDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, accessor);

        await Should.ThrowAsync<UnauthorizedAccessException>(async () => await handler.Handle(command, CancellationToken.None));
 
    }



    [Fact]
    public async Task UpdatePlaneDraft_AllValid_ShouldReturnUnit()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        var planeId = Guid.NewGuid();
        _planeMock.GetById(planeId).ReturnsForAnyArgs(new Plane(planeId,  new ReadOnlyCollection<Seat>(new List<Seat>())));
        
        var command = new UpdatePlaneDraftCommand(Guid.NewGuid(), planeId);
        
        var handler = new UpdatePlaneDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, GetAccessor(), _planeMock);

        var result = await handler.Handle(command, CancellationToken.None);
        
        await _reservationRepositoryMock.Received(1).Update(draft, Arg.Any<CancellationToken>());
        await _mockEventProcessor.Received(1).ProcessAsync(draft, Arg.Any<CancellationToken>());
        
        draft.Plane.Id.ShouldBe(planeId);
        result.ShouldBeOfType<Unit>();
    }
    
    [Fact]
    public async Task UpdatePlaneDraft_InvalidClaims_ShouldReturnUnit()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        var planeId = Guid.NewGuid();
        _planeMock.GetById(planeId).ReturnsForAnyArgs(new Plane(planeId,  new ReadOnlyCollection<Seat>(new List<Seat>())));

        
        var command = new UpdatePlaneDraftCommand(Guid.NewGuid(), planeId);
        
        var claims = new Dictionary<string, string>{{"User","User"}};
        var accessor = GetAccessor(claims:claims);
        
        var handler = new UpdatePlaneDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, accessor, _planeMock);

        await Should.ThrowAsync<UnauthorizedAccessException>(async () => await handler.Handle(command, CancellationToken.None));
 
    }
    
    [Fact]
    public async Task UpdatePlaneDraft_PlaneNotExist_ShouldReturnUnit()
    {
        var draft = GetValidDraft();
        _reservationRepositoryMock.GetDraftById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(draft);
        
        var planeId = Guid.NewGuid();
        
        var command = new UpdatePlaneDraftCommand(Guid.NewGuid(), planeId);
        
        var handler = new UpdatePlaneDraftCommandHandler(_reservationRepositoryMock, _mockEventProcessor, GetAccessor(), _planeMock);
        

        await Should.ThrowAsync<DraftCannotBeCreatedException>(async () => await handler.Handle(command, CancellationToken.None));
 
    }









    private Reservations.Core.Entities.ReservationDraft GetValidDraft()
    {
        var airport = new AirportDto("New York CIty Airport", "New York", "JFK");
        
        var plane = new Plane(Guid.NewGuid(), new ReadOnlyCollection<Seat>(new List<Seat>()));
        
        return ReservationDraft.Create(new Id(Guid.NewGuid()), new Journey(airport, airport), 
            new FlightDate(_clock.CurrentDateTime,_clock.CurrentDateTime.AddHours(8)), new Worker(Guid.NewGuid()), plane);
    }
    
    
    private IUserContextAccessor GetAccessor(string role = "Worker", Dictionary<string,string> claims = null)
    {
        if (claims is null)
        {
            claims = new Dictionary<string, string>
            {
                { "Worker", "Worker" }
            };
        }
        
        return new UserContextAccessorTest(Guid.NewGuid(), role,
            claims);
    }
}