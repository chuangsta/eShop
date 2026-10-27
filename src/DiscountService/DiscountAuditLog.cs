namespace eShop.DiscountService;

public static partial class DiscountAuditLog
{
    public const string TimestampFormat = "yyyy-MM-dd HH:mm:ss";

    public static string FormatLine(DateTime timestamp, DiscountResult result) =>
        timestamp.ToString(TimestampFormat) + " " + result.ToAuditRecord();

    public static void Write(ILogger logger, DiscountResult result) =>
        logger.DiscountCalculated(
            DateTime.Now.ToString(TimestampFormat),
            result.ToAuditRecord(),
            result.CustomerId,
            result.Amount,
            result.DiscountPercent,
            result.DiscountedAmount);

    // The rendered message must equal the legacy audit line exactly, so the per-value fields stay out of the
    // template; the generator still emits them as structured state.
#pragma warning disable SYSLIB1015
    [LoggerMessage(
        EventId = 1,
        EventName = "DiscountCalculated",
        Level = LogLevel.Information,
        Message = "{Timestamp} {AuditRecord}")]
    public static partial void DiscountCalculated(
        this ILogger logger,
        string timestamp,
        string auditRecord,
        string? customerId,
        decimal amount,
        decimal discountPercent,
        decimal discountedAmount);
#pragma warning restore SYSLIB1015
}
