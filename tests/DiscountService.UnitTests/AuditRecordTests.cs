using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace eShop.DiscountService.UnitTests;

public class AuditRecordTests
{
    private static readonly DiscountResult KnownResult = new("PREF-0001", 250m, 18m, 205m);

    [Fact]
    public void ToAuditRecordProducesPipeFormatWithEnUsCulture()
    {
        using var culture = new CultureScope("en-US");

        Assert.Equal("PREF-0001|250.00|18.0|205.00", KnownResult.ToAuditRecord());
    }

    [Fact]
    public void FormatLineProducesLegacyTimestampedAuditLine()
    {
        using var culture = new CultureScope("en-US");

        var line = DiscountAuditLog.FormatLine(new DateTime(2024, 3, 5, 14, 7, 9), KnownResult);

        Assert.Equal("2024-03-05 14:07:09 PREF-0001|250.00|18.0|205.00", line);
    }

    [Fact]
    public void CapturedLogEntryIsInformationWithStructuredFieldsAndLegacyLine()
    {
        using var culture = new CultureScope("en-US");
        var logger = new CapturingLogger();

        DiscountAuditLog.Write(logger, KnownResult);

        var entry = Assert.Single(logger.Entries);
        Assert.Equal(LogLevel.Information, entry.Level);
        Assert.Matches(
            new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2} PREF-0001\|250\.00\|18\.0\|205\.00$"),
            entry.Message);

        var fields = entry.State.ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        Assert.Matches(new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$"), Assert.IsType<string>(fields["Timestamp"]));
        Assert.Equal("PREF-0001|250.00|18.0|205.00", fields["AuditRecord"]);
        Assert.Equal("PREF-0001", fields["customerId"]);
        Assert.Equal(250m, fields["amount"]);
        Assert.Equal(18m, fields["discountPercent"]);
        Assert.Equal(205m, fields["discountedAmount"]);
    }
}
