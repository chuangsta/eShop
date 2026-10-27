namespace eShop.DiscountService;

public sealed class DiscountOptions
{
    public const string SectionName = "Discount";

    public decimal LoyaltyYearsPercent { get; set; } = 1m;

    public decimal PreferredCustomerPercent { get; set; } = 5m;

    public string PreferredCustomerPrefix { get; set; } = "PREF";

    public decimal VolumeThreshold { get; set; } = 100m;

    public decimal VolumePercent { get; set; } = 10m;

    public decimal MaxPercent { get; set; } = 20m;

    public int RoundingDigits { get; set; } = 2;

    public TimeSpan SimulatedMainframeDelay { get; set; } = TimeSpan.FromMilliseconds(150);

    // decimal.Round only accepts 0..28 fractional digits.
    public bool IsValid() =>
        RoundingDigits is >= 0 and <= 28
        && SimulatedMainframeDelay >= TimeSpan.Zero
        && MaxPercent is >= 0m and <= 100m
        && PreferredCustomerPrefix is not null;
}
