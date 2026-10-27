namespace eShop.DiscountService.UnitTests;

public class VolumeRuleTests
{
    private readonly DiscountCalculator _calculator = TestCalculator.Create();

    [Fact]
    public void AmountJustBelowThreshold99_99GetsNoVolumeBonus()
    {
        var result = _calculator.Calculate("C1", 99.99m, 0);

        Assert.Equal(0m, result.DiscountPercent);
        Assert.Equal(99.99m, result.DiscountedAmount);
    }

    [Fact]
    public void AmountExactly100GetsVolumeBonusBecauseThresholdIsInclusive()
    {
        var result = _calculator.Calculate("C1", 100m, 0);

        Assert.Equal(10m, result.DiscountPercent);
        Assert.Equal(90.00m, result.DiscountedAmount);
    }

    [Fact]
    public void Amount250GetsVolumeBonus()
    {
        var result = _calculator.Calculate("C1", 250m, 0);

        Assert.Equal(10m, result.DiscountPercent);
        Assert.Equal(225.00m, result.DiscountedAmount);
    }
}
