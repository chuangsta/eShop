namespace eShop.DiscountService.UnitTests;

public class LoyaltyRuleTests
{
    private readonly DiscountCalculator _calculator = TestCalculator.Create();

    [Fact]
    public void ZeroLoyaltyYearsGivesZeroPercent()
    {
        var result = _calculator.Calculate("C1", 50m, 0);

        Assert.Equal(0m, result.DiscountPercent);
        Assert.Equal(50.00m, result.DiscountedAmount);
    }

    [Fact]
    public void OneLoyaltyYearGivesOnePercent()
    {
        var result = _calculator.Calculate("C1", 50m, 1);

        Assert.Equal(1m, result.DiscountPercent);
        Assert.Equal(49.50m, result.DiscountedAmount);
    }

    [Fact]
    public void ThreeLoyaltyYearsGiveThreePercent()
    {
        var result = _calculator.Calculate("C1", 50m, 3);

        Assert.Equal(3m, result.DiscountPercent);
        Assert.Equal(48.50m, result.DiscountedAmount);
    }

    [Fact]
    public void FifteenLoyaltyYearsAloneGiveFifteenPercentUnderTheCap()
    {
        var result = _calculator.Calculate("C1", 50m, 15);

        Assert.Equal(15m, result.DiscountPercent);
        Assert.Equal(42.50m, result.DiscountedAmount);
    }

    [Fact]
    public void NegativeLoyaltyYearsAreNotClampedAndRaiseThePayableAmount()
    {
        var result = _calculator.Calculate("C1", 50m, -3);

        Assert.Equal(-3m, result.DiscountPercent);
        Assert.Equal(51.50m, result.DiscountedAmount);
    }
}
