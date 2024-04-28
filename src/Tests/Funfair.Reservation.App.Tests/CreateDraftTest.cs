using System.Collections.ObjectModel;
using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.Core;
using Funfair.Shared.Domain;
using Moq;
using NSubstitute;
using Reservations.App.Commands.CreateDraft;
using Reservations.App.Exceptions;
using Reservations.App.Services;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;
using Shouldly;
using Test.Shared;

namespace Funfair.Reservation.App.Tests;

public class CreateDraftTest
{
    private readonly IReservationRepository _reservationRepositoryMock = Substitute.For<IReservationRepository>();
    private readonly IEventProcessor _mockEventProcessor =  Substitute.For<IEventProcessor>();
    private readonly IPlaneService _planeMock = Substitute.For<IPlaneService>();
    private readonly IClock _clock = new ClockTest();
    
    
    
    [Fact]
    public async Task CreateDraft_ValidAll_ShouldReturnDraftId()
    {
        var planeId = Guid.NewGuid();

        var request = GetCreateCommand(planeId);    
        _planeMock.GetById(planeId).ReturnsForAnyArgs(new Plane(planeId,  new ReadOnlyCollection<Seat>(new List<Seat>())));
        
        var handler = new CreateDraftCommandHandler(_planeMock, GetAccessor(), _reservationRepositoryMock, _mockEventProcessor);

        var result =  await handler.Handle(request, CancellationToken.None);
        
        await _planeMock.Received(1).GetById(planeId, Arg.Any<CancellationToken>());
        await _reservationRepositoryMock.Received(1).Create(Arg.Any<ReservationDraft>(), Arg.Any<CancellationToken>());
        await _mockEventProcessor.Received(1).ProcessAsync(Arg.Any<DomainBase>(), Arg.Any<CancellationToken>());
        
        result.Id.ShouldNotBe(Guid.Empty);


    }
    
    [Fact]
    public async Task CreateDraft_PlaneNotExists_ShouldThrowAnException()
    {
        var planeId = Guid.NewGuid();

        var request = GetCreateCommand(planeId);    
        
        var handler = new CreateDraftCommandHandler(_planeMock, GetAccessor(), _reservationRepositoryMock, _mockEventProcessor);

        await Should.ThrowAsync<DraftCannotBeCreatedException>(async () => await handler.Handle(request, CancellationToken.None));
    }
     
    [Fact]
    public async Task CreateDraft_UserNotHaveWorkerClaim_ShouldThrowAnException()
    {
        var planeId = Guid.NewGuid();

        var request = GetCreateCommand(planeId);    
        var claims = new Dictionary<string, string>{{"User","User"}};
        
        var handler = new CreateDraftCommandHandler(_planeMock, GetAccessor(claims: claims), _reservationRepositoryMock, _mockEventProcessor);

        await Should.ThrowAsync<UnauthorizedAccessException>(async () => await handler.Handle(request, CancellationToken.None));
    }
    
    

    private CreateDraftCommand GetCreateCommand(Guid planeId)
    {
        return new CreateDraftCommand(
            new AirportDto("Katowice Airport", "Katowice", "KTW"),
            new AirportDto("New York CIty Airport", "New York", "JFK"),
            planeId,
            _clock.CurrentDateTime,
            _clock.CurrentDateTime.AddHours(10)
        );
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