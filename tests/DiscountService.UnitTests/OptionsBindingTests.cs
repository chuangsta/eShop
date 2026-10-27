using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace eShop.DiscountService.UnitTests;

public class OptionsBindingTests
{
    private static DiscountOptions Bind(Dictionary<string, string?> values)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddOptions<DiscountOptions>().BindConfiguration(DiscountOptions.SectionName);

        using var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IOptions<DiscountOptions>>().Value;
    }

    [Fact]
    public void InMemoryConfigurationBindsEveryDiscountFieldIncludingDelayAndPrefix()
    {
        var options = Bind(new Dictionary<string, string?>
        {
            ["Discount:LoyaltyYearsPercent"] = "1.5",
            ["Discount:PreferredCustomerPercent"] = "7",
            ["Discount:PreferredCustomerPrefix"] = "VIP",
            ["Discount:VolumeThreshold"] = "500",
            ["Discount:VolumePercent"] = "12",
            ["Discount:MaxPercent"] = "30",
            ["Discount:RoundingDigits"] = "3",
            ["Discount:SimulatedMainframeDelay"] = "00:00:00.150",
        });

        Assert.Equal(1.5m, options.LoyaltyYearsPercent);
        Assert.Equal(7m, options.PreferredCustomerPercent);
        Assert.Equal("VIP", options.PreferredCustomerPrefix);
        Assert.Equal(500m, options.VolumeThreshold);
        Assert.Equal(12m, options.VolumePercent);
        Assert.Equal(30m, options.MaxPercent);
        Assert.Equal(3, options.RoundingDigits);
        Assert.Equal(TimeSpan.FromMilliseconds(150), options.SimulatedMainframeDelay);
    }

    [Fact]
    public void AbsentDiscountSectionLeavesCodedDefaults()
    {
        var options = Bind([]);

        Assert.Equal(1m, options.LoyaltyYearsPercent);
        Assert.Equal(5m, options.PreferredCustomerPercent);
        Assert.Equal("PREF", options.PreferredCustomerPrefix);
        Assert.Equal(100m, options.VolumeThreshold);
        Assert.Equal(10m, options.VolumePercent);
        Assert.Equal(20m, options.MaxPercent);
        Assert.Equal(2, options.RoundingDigits);
        Assert.Equal(TimeSpan.FromMilliseconds(150), options.SimulatedMainframeDelay);
    }
}
