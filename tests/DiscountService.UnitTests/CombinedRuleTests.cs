namespace eShop.DiscountService.UnitTests;

public class CombinedRuleTests
{
    private readonly DiscountCalculator _calculator = TestCalculator.Create();

    [Fact]
    public void Pref0001With250And3YearsGives18PercentAnd205()
    {
        var result = _calculator.Calculate("PREF-0001", 250m, 3);

        Assert.Equal("PREF-0001", result.CustomerId);
        Assert.Equal(250m, result.Amount);
        Assert.Equal(18m, result.DiscountPercent);
        Assert.Equal(205.00m, result.DiscountedAmount);
    }

    [Fact]
    public void TenYearsPrefAnd250CapsAt20PercentAndGives200()
    {
        var result = _calculator.Calculate("PREF", 250m, 10);

        Assert.Equal(20m, result.DiscountPercent);
        Assert.Equal(200.00m, result.DiscountedAmount);
    }
}
