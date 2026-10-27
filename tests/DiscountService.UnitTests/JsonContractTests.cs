using System.Text.Json;
using System.Text.Json.Serialization;

namespace eShop.DiscountService.UnitTests;

public class JsonContractTests
{
    // Mirrors the HTTP JSON options configured in Program.cs.
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    [Fact]
    public void DiscountResultSerializesToExactContractPropertyNamesWithNumericValues()
    {
        var json = JsonSerializer.Serialize(new DiscountResult("PREF-0001", 250m, 18m, 205m), SerializerOptions);

        using var document = JsonDocument.Parse(json);
        var properties = document.RootElement.EnumerateObject().ToList();

        Assert.Equal(
            ["customerId", "amount", "discountPercent", "discountedAmount"],
            properties.Select(p => p.Name).ToArray());
        Assert.Equal("PREF-0001", document.RootElement.GetProperty("customerId").GetString());
        Assert.Equal(JsonValueKind.Number, document.RootElement.GetProperty("amount").ValueKind);
        Assert.Equal(JsonValueKind.Number, document.RootElement.GetProperty("discountPercent").ValueKind);
        Assert.Equal(JsonValueKind.Number, document.RootElement.GetProperty("discountedAmount").ValueKind);
        Assert.Equal(205m, document.RootElement.GetProperty("discountedAmount").GetDecimal());
    }

    [Fact]
    public void DiscountRequestDeserializesContractPropertyNames()
    {
        var request = JsonSerializer.Deserialize<DiscountRequest>(
            """{"customerId":"PREF-0001","amount":250.00,"loyaltyYears":3}""",
            SerializerOptions);

        Assert.Equal(new DiscountRequest("PREF-0001", 250.00m, 3), request);
    }

    [Fact]
    public void DiscountRequestAcceptsQuotedNumbersLikeNewtonsoft()
    {
        var quoted = JsonSerializer.Deserialize<DiscountRequest>(
            """{"customerId":"PREF-0001","amount":"250.00","loyaltyYears":"3"}""",
            SerializerOptions);
        var unquoted = JsonSerializer.Deserialize<DiscountRequest>(
            """{"customerId":"PREF-0001","amount":250.00,"loyaltyYears":3}""",
            SerializerOptions);

        Assert.Equal(unquoted, quoted);
        Assert.Equal(250.00m, quoted!.Amount);
        Assert.Equal(3, quoted.LoyaltyYears);
    }
}
