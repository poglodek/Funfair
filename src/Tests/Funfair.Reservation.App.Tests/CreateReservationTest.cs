using System.Collections.ObjectModel;
using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Auth;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core;
using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;
using MediatR;
using NSubstitute;
using Reservations.App.Commands.CreateDraft;
using Reservations.App.Commands.CreateReservation;
using Reservations.App.Exceptions;
using Reservations.Core.Entities;
using Reservations.Core.Events;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;
using Shouldly;
using Test.Shared;

namespace Funfair.Reservation.App.Tests;

public class CreateReservationTest
{
    private readonly IReservationRepository _reservationRepositoryMock = Substitute.For<IReservationRepository>();
    private readonly IEventDispatcher eventProcessor =  new EventProcessorTest();
    private readonly IClock _clock = new ClockTest();


    [Fact]
    public async Task CreateReservation_AllValid_ShouldReturnUnitValue()
    {
        var draftId = Guid.NewGuid();
        
        var command = new CreateReservationCommand(draftId, new PriceDto(1000, "USD"));
        
        _reservationRepositoryMock.GetDraftById(draftId).Returns(GetValidDraft(draftId));
        
        var handler = new CreateReservationCommandHandler(_reservationRepositoryMock, GetAccessor(), _clock, eventProcessor );
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        await _reservationRepositoryMock.Received(1).AddNewReservation(Arg.Any<Reservations.Core.Entities.Reservation>(), Arg.Any<ReservationDraft>(), Arg.Any<CancellationToken>());

        var domainEvents = ((EventProcessorTest)eventProcessor).DomainEvents;
        
        domainEvents.Count.ShouldBe(1);
        domainEvents[0].ShouldBeOfType<NewReservationCreatedEvent>();
        
        var newReservationCreatedEvent = (NewReservationCreatedEvent)domainEvents[0];
        
        newReservationCreatedEvent.Id.Value.ShouldBe(draftId);

        result.ShouldBeOfType<Unit>();
    }
    
    
    [Fact]
    public async Task CreateReservation_DraftNotExists_ShouldThrowAnException()
    {
        var draftId = Guid.NewGuid();
        
        var command = new CreateReservationCommand(draftId, new PriceDto(1000, "USD"));
        
        
        var handler = new CreateReservationCommandHandler(_reservationRepositoryMock, GetAccessor(), _clock, eventProcessor );
        
        await Should.ThrowAsync<DraftNotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        
      
    }
    
    
    
    
    
    private Reservations.Core.Entities.ReservationDraft GetValidDraft(Guid id)
    {
        var airport = new AirportDto("New York CIty Airport", "New York", "JFK");
        
        var plane = new Plane(id, new ReadOnlyCollection<Seat>(new List<Seat>()));
        
        return ReservationDraft.Create(new Id(id), new Journey(airport, airport), 
            new FlightDate(_clock.CurrentDateTime.AddDays(4),_clock.CurrentDateTime.AddDays(4).AddHours(8)), new Worker(Guid.NewGuid()), plane);
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