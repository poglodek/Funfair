using NSubstitute;
using Planes.App.Commands;
using Planes.App.Commands.Dto;
using Planes.App.Commands.Handlers;
using Planes.App.Exceptions;
using Planes.App.Repositories;
using Planes.Core.Exceptions;
using Planes.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Funfair.App.Plane;

public class CreatePlaneCommandTest
{
    private Task<PlaneIdDto> Act(CreatePlaneCommand command)
    {
        var handler = new CreatePlaneCommandHandler(_userRepository);
        return handler.Handle(command, default);
    }
    
    [Fact]
    public async Task CreatePlane_AllValid_ShouldReturnPlaneId()
    {
        var command = new CreatePlaneCommand("Boeing 737", 2010, new List<Seats>
        {
            new Seats(10, 6, "Economy"),
            new Seats(5, 4, "Business")
        });
        
        var response = await Act(command);
        
        response.ShouldNotBeNull();
        response.Id.ShouldNotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task CreatePlane_WithExistingModel_ShouldThrowPlaneExistsException()
    {
        var command = new CreatePlaneCommand("Boeing 737", 2010, new List<Seats>
        {
            new Seats(10, 6, "Economy"),
            new Seats(5, 4, "Business")
        });
        
        _userRepository.GetByModelAsync("Boeing 737").Returns(ReturnPlane());
        
        var exception = await Record.ExceptionAsync(()=>Act(command));
        
        exception.ShouldBeOfType<PlaneExistsException>();
    }
    
    [Fact]
    public async Task CreatePlane_WithNullSeats_ShouldReturnPlaneId()
    {
        var command = new CreatePlaneCommand("Boeing 737", 2010, null);
        
        Should.Throw<InvalidSeatsException>(async () => await Act(command));

    }


    private readonly IPlaneRepository _userRepository = Substitute.For<IPlaneRepository>();
    
    private Planes.Core.Entities.Plane ReturnPlane() =>  Planes.Core.Entities.Plane.Create(Guid.NewGuid(),"Boeing 737", 2010, new List<Seat>
    {
        new Seat(4, "A", SeatClass.Business),
        new Seat(4, "B", SeatClass.Business),
        new Seat(4, "C", SeatClass.Business),
    });

}