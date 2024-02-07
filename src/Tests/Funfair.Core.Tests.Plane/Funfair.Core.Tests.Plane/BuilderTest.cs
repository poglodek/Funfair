using Planes.Core.Builder;
using Planes.Core.Events;
using Planes.Core.Exceptions;
using Planes.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Funfair.Core.Tests.Plane;

public class BuilderTest
{
    [Fact]
    public void CreatePlane_AllValid_ReturnPlane()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = "Airbus A320";
        var productionYear = 2020;
        var economySeatsRow = 10;
        var economySeatsInRow = 6;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WitId(id)
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        var plane = planeBuilder.Build();
        
        
        // Assert
        plane.Id.Value.ShouldBe(id);
        plane.Model.Value.ShouldBe(model);
        plane.ProductionYear.Year.ShouldBe(productionYear);
        plane.Seats.Count(x => x.SeatClass == SeatClass.Economy).ShouldBe(economySeatsRow * economySeatsInRow);
        plane.Seats.Count(x => x.SeatClass == SeatClass.Premium).ShouldBe(premiumSeatsRow * premiumSeatsInRow);
        plane.Seats.Count(x => x.SeatClass == SeatClass.Business).ShouldBe(businessSeatsRow * businessSeatsInRow);
    }
    
    [Fact]
    public void CreatePlane_InvalidModel_ThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = "";
        var productionYear = 2020;
        var economySeatsRow = 10;
        var economySeatsInRow = 6;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WitId(id)
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        // Assert
        Should.Throw<InvalidPlaneNameException>(() => planeBuilder.Build());
    }
    
    [Fact]
    public void CreatePlane_InvalidProductionYear_ThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = "Airbus A320";
        var productionYear = 0;
        var economySeatsRow = 10;
        var economySeatsInRow = 6;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WitId(id)
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        // Assert
        Should.Throw<InvalidProductionYearException>(() => planeBuilder.Build());
    }
    
    [Fact]
    public void CreatePlane_InvalidSeats_ThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = "Airbus A320";
        var productionYear = 2020;
        var economySeatsRow = 0;
        var economySeatsInRow = 6;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WitId(id)
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        // Assert
        Should.Throw<InvalidSeatsInRowException>(() => planeBuilder.WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy));
    }
    
    [Fact]
    public void CreatePlane_InvalidSeatsInRow_ThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = "Airbus A320";
        var productionYear = 2020;
        var economySeatsRow = 10;
        var economySeatsInRow = 10;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WitId(id)
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        // Assert
        Should.Throw<InvalidSeatsInRowException>(() => planeBuilder.WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy));
    }
    
    [Fact]
    public void CreatePlane_InvalidId_ThrowException()
    {
        // Arrange
        var id = Guid.Empty;
        var model = "Airbus A320";
        var productionYear = 2020;
        var economySeatsRow = 10;
        var economySeatsInRow = 6;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WitId(id)
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        // Assert
        Should.Throw<InvalidIdException>(() => planeBuilder.Build());
    }
    
    [Fact]
    public void CreatePlane_InvalidIdEmpty_ShouldCreateWithNewId()
    {
        // Arrange
        var id = Guid.Empty;
        var model = "Airbus A320";
        var productionYear = 2020;
        var economySeatsRow = 10;
        var economySeatsInRow = 6;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        // Assert
        var plane = planeBuilder.Build();
        plane.Id.Value.ShouldNotBe(Guid.Empty);
    }
    
    [Fact]
    public void CreatePlane_InvalidModelNull_ThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = (string?)null;
        var productionYear = 2020;
        var economySeatsRow = 10;
        var economySeatsInRow = 6;
        var premiumSeatsRow = 5;
        var premiumSeatsInRow = 2;
        var businessSeatsRow = 3;
        var businessSeatsInRow = 2;
        
        // Act
        var planeBuilder = new PlaneBuilder()
            .WitId(id)
            .WithModel(model)
            .WithProductionYear(productionYear)
            .WithSeats(economySeatsRow, economySeatsInRow, SeatClass.Economy)
            .WithSeats(premiumSeatsRow, premiumSeatsInRow, SeatClass.Premium)
            .WithSeats(businessSeatsRow, businessSeatsInRow, SeatClass.Business);
        
        // Assert
        Should.Throw<InvalidPlaneBuilderProperty>(() => planeBuilder.Build());
    }
    
}