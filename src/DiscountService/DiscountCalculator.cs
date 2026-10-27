using Microsoft.Extensions.Options;

namespace eShop.DiscountService;

public sealed class DiscountCalculator(IOptions<DiscountOptions> options)
{
    private readonly DiscountOptions _options = options.Value;

    public DiscountResult Calculate(string? customerId, decimal amount, int loyaltyYears)
    {
        var percent = _options.LoyaltyYearsPercent * loyaltyYears;

        if (customerId is not null && customerId.StartsWith(_options.PreferredCustomerPrefix, StringComparison.Ordinal))
        {
            percent += _options.PreferredCustomerPercent;
        }

        if (amount >= _options.VolumeThreshold)
        {
            percent += _options.VolumePercent;
        }

        if (percent > _options.MaxPercent)
        {
            percent = _options.MaxPercent;
        }

        var discountedAmount = decimal.Round(
            amount * (100m - percent) / 100m,
            _options.RoundingDigits,
            MidpointRounding.AwayFromZero);

        return new DiscountResult(customerId, amount, percent, discountedAmount);
    }
}
