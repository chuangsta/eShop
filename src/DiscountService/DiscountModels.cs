using System.Text.Json.Serialization;

namespace eShop.DiscountService;

public sealed record DiscountRequest(
    [property: JsonPropertyName("customerId")] string? CustomerId,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("loyaltyYears")] int LoyaltyYears);

public sealed record DiscountResult(
    [property: JsonPropertyName("customerId")] string? CustomerId,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("discountPercent")] decimal DiscountPercent,
    [property: JsonPropertyName("discountedAmount")] decimal DiscountedAmount)
{
    // Pipe-delimited format consumed by the legacy batch import; uses the current culture like the original.
    public string ToAuditRecord() =>
        CustomerId
        + "|" + Amount.ToString("0.00")
        + "|" + DiscountPercent.ToString("0.0")
        + "|" + DiscountedAmount.ToString("0.00");
}
