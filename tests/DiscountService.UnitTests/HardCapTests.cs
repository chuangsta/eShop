namespace eShop.DiscountService.UnitTests;

public class HardCapTests
{
    [Fact]
    public void TotalJustUnderCap19StaysAt19()
    {
        var result = TestCalculator.Create().Calculate("C1", 100m, 9);

        Assert.Equal(19m, result.DiscountPercent);
        Assert.Equal(81.00m, result.DiscountedAmount);
    }

    [Fact]
    public void TotalExactlyAtCap20StaysAt20()
    {
        var result = TestCalculator.Create().Calculate("PREF-0001", 250m, 5);

        Assert.Equal(20m, result.DiscountPercent);
        Assert.Equal(200.00m, result.DiscountedAmount);
    }

    [Fact]
    public void TotalAboveCap25From10YearsPrefAnd250IsCappedAt20()
    {
        var result = TestCalculator.Create().Calculate("PREF-0001", 250m, 10);

        Assert.Equal(20m, result.DiscountPercent);
    }

    [Fact]
    public void ConfiguredCap15CapsTotalAt15()
    {
        var calculator = TestCalculator.Create(o => o.MaxPercent = 15m);

        var result = calculator.Calculate("PREF-0001", 250m, 10);

        Assert.Equal(15m, result.DiscountPercent);
        Assert.Equal(212.50m, result.DiscountedAmount);
    }
}
