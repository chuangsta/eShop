using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eShop.DiscountService.UnitTests;

public class EndpointTests
{
    private readonly IOptions<DiscountOptions> _options =
        TestCalculator.CreateOptions(o => o.SimulatedMainframeDelay = TimeSpan.Zero);

    private readonly CapturingLogger _logger = new();

    private DiscountCalculator Calculator => new(_options);

    [Fact]
    public async Task ValidGetReturnsOkWithExpectedDiscountResult()
    {
        var response = await DiscountEndpoints.GetDiscount(
            null, "PREF-0001", 250m, 3, Calculator, _options, _logger, TestContext.Current.CancellationToken);

        var ok = Assert.IsType<Ok<DiscountResult>>(response.Result);
        Assert.Equal(new DiscountResult("PREF-0001", 250m, 18m, 205.00m), ok.Value);
        var entry = Assert.Single(_logger.Entries);
        Assert.Equal(LogLevel.Information, entry.Level);
    }

    [Fact]
    public async Task GetWithNullCustomerIdReturnsBadRequestWithoutAudit()
    {
        var response = await DiscountEndpoints.GetDiscount(
            null, null, 250m, 3, Calculator, _options, _logger, TestContext.Current.CancellationToken);

        Assert.IsType<BadRequest>(response.Result);
        Assert.Empty(_logger.Entries);
    }

    [Fact]
    public async Task GetWithWhitespaceCustomerIdReturnsBadRequestWithoutAudit()
    {
        var response = await DiscountEndpoints.GetDiscount(
            null, "   ", 250m, 3, Calculator, _options, _logger, TestContext.Current.CancellationToken);

        Assert.IsType<BadRequest>(response.Result);
        Assert.Empty(_logger.Entries);
    }

    [Fact]
    public async Task PostWithNullBodyReturnsBadRequestWithoutAudit()
    {
        var response = await DiscountEndpoints.PostDiscount(null, null, Calculator, _logger);

        Assert.IsType<BadRequest>(response.Result);
        Assert.Empty(_logger.Entries);
    }

    [Fact]
    public async Task ValidPostReturnsOkWithExpectedValuesAndOneAuditEntry()
    {
        var response = await DiscountEndpoints.PostDiscount(
            null, new DiscountRequest("PREF-0001", 250m, 3), Calculator, _logger);

        var ok = Assert.IsType<Ok<DiscountResult>>(response.Result);
        Assert.Equal(new DiscountResult("PREF-0001", 250m, 18m, 205.00m), ok.Value);
        var entry = Assert.Single(_logger.Entries);
        Assert.Equal(LogLevel.Information, entry.Level);
    }
}
