namespace eShop.DiscountService.UnitTests;

public class PreferredCustomerRuleTests
{
    private readonly DiscountCalculator _calculator = TestCalculator.Create();

    [Fact]
    public void ExactPrefixPrefMatchesAndAddsFivePercent()
    {
        Assert.Equal(5m, _calculator.Calculate("PREF", 50m, 0).DiscountPercent);
    }

    [Fact]
    public void PrefixedIdPref0001MatchesAndAddsFivePercent()
    {
        Assert.Equal(5m, _calculator.Calculate("PREF-0001", 50m, 0).DiscountPercent);
    }

    [Fact]
    public void LowercasePref1DoesNotMatchBecauseComparisonIsCaseSensitive()
    {
        Assert.Equal(0m, _calculator.Calculate("pref-1", 50m, 0).DiscountPercent);
    }

    [Fact]
    public void ShorterIdPreDoesNotMatch()
    {
        Assert.Equal(0m, _calculator.Calculate("PRE", 50m, 0).DiscountPercent);
    }

    [Fact]
    public void PrefNotAtStartXprefDoesNotMatch()
    {
        Assert.Equal(0m, _calculator.Calculate("XPREF", 50m, 0).DiscountPercent);
    }

    [Fact]
    public void EmptyCustomerIdDoesNotMatch()
    {
        Assert.Equal(0m, _calculator.Calculate(string.Empty, 50m, 0).DiscountPercent);
    }

    [Fact]
    public void NullCustomerIdDoesNotMatch()
    {
        Assert.Equal(0m, _calculator.Calculate(null, 50m, 0).DiscountPercent);
    }
}
