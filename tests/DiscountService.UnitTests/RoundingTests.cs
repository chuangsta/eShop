namespace eShop.DiscountService.UnitTests;

public class RoundingTests
{
    private readonly DiscountCalculator _calculator = TestCalculator.Create();

    [Fact]
    public void PositiveMidpoint9_595RoundsAwayFromZeroTo9_60()
    {
        Assert.Equal(9.60m, _calculator.Calculate("C1", 9.595m, 0).DiscountedAmount);
    }

    [Fact]
    public void NegativeMidpointMinus9_595RoundsAwayFromZeroToMinus9_60()
    {
        Assert.Equal(-9.60m, _calculator.Calculate("C1", -9.595m, 0).DiscountedAmount);
    }

    [Fact]
    public void NonMidpointAmountIsUnchanged()
    {
        Assert.Equal(12.34m, _calculator.Calculate("C1", 12.34m, 0).DiscountedAmount);
    }

    [Fact]
    public void MidpointProducedByDiscount10_10At5PercentRoundsTo9_60()
    {
        var result = _calculator.Calculate("PREF", 10.10m, 0);

        Assert.Equal(5m, result.DiscountPercent);
        Assert.Equal(9.60m, result.DiscountedAmount);
    }

    [Fact]
    public void PercentIsNeverRoundedHalfPercentTimesThreeYearsIsExactly1_5()
    {
        var calculator = TestCalculator.Create(o => o.LoyaltyYearsPercent = 0.5m);

        var result = calculator.Calculate("C1", 50m, 3);

        Assert.Equal(1.5m, result.DiscountPercent);
        Assert.Equal(decimal.Round(50m * 98.5m / 100m, 2, MidpointRounding.AwayFromZero), result.DiscountedAmount);
        Assert.Equal(49.25m, result.DiscountedAmount);
    }
}
