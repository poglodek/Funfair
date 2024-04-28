using Xunit;
using Reservations.Core.Policy;
using Reservations.Core.ValueObjects;
using System;

public class PolicyPriceTests
{
    [Fact]
    public void CalculatePrice_ShouldReturnCorrectPrice_ForEconomyClassSeat()
    {
        var policyPrice = new PolicyPrice();
        var seat = new Seat (Guid.NewGuid(),"1","A",ClassSeat.Economy );
        var standardPrice = new Price (100, "USD");

        var result = policyPrice.CalculatePrice(seat, standardPrice);

        Assert.Equal(100, result.Value);
    }

    [Fact]
    public void CalculatePrice_ShouldReturnCorrectPrice_ForPremiumClassSeat()
    {
        var policyPrice = new PolicyPrice();
        var seat = new Seat (Guid.NewGuid(),"1","A",ClassSeat.Premium );
        var standardPrice =  new Price (100, "USD");

        var result = policyPrice.CalculatePrice(seat, standardPrice);

        Assert.Equal(175, result.Value);
    }

    [Fact]
    public void CalculatePrice_ShouldReturnCorrectPrice_ForBusinessClassSeat()
    {
        var policyPrice = new PolicyPrice();
        var seat = new Seat (Guid.NewGuid(),"1","A",ClassSeat.Business );
        var standardPrice =  new Price (100, "USD");

        var result = policyPrice.CalculatePrice(seat, standardPrice);

        Assert.Equal(222, result.Value);
    }
    
    [Fact]
    public void CalculateInStandardPrice_ShouldReturnCorrectPrice_ForBusinessClassSeat()
    {
        var policyPrice = new PolicyPrice();
        var seat = new Seat (Guid.NewGuid(),"1","A",ClassSeat.Business );
        var standardPrice =  new Price (237.77, "USD");

        var result = policyPrice.CalculatePrice(seat, standardPrice);

        Assert.Equal(527.85, result.Value);
    }

    [Fact]
    public void CalculatePrice_ShouldThrowException_WhenSeatIsNull()
    {
        var policyPrice = new PolicyPrice();
        var standardPrice =  new Price (100, "USD");

        Assert.Throws<ArgumentNullException>(() => policyPrice.CalculatePrice(null, standardPrice));
    }

    [Fact]
    public void CalculatePrice_ShouldThrowException_WhenStandardPriceIsNull()
    {
        var policyPrice = new PolicyPrice();
        var seat = new Seat (Guid.NewGuid(),"1","A",ClassSeat.Economy );

        Assert.Throws<ArgumentNullException>(() => policyPrice.CalculatePrice(seat, null));
    }
}
