using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eShop.DiscountService.UnitTests;

internal static class TestCalculator
{
    public static DiscountCalculator Create(Action<DiscountOptions>? configure = null) =>
        new(CreateOptions(configure));

    public static IOptions<DiscountOptions> CreateOptions(Action<DiscountOptions>? configure = null)
    {
        var options = new DiscountOptions();
        configure?.Invoke(options);
        return Options.Create(options);
    }
}

internal sealed record CapturedLogEntry(
    LogLevel Level,
    EventId EventId,
    string Message,
    IReadOnlyList<KeyValuePair<string, object?>> State);

internal sealed class CapturingLogger : ILogger
{
    public List<CapturedLogEntry> Entries { get; } = [];

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var fields = state as IReadOnlyList<KeyValuePair<string, object?>> ?? [];
        Entries.Add(new CapturedLogEntry(logLevel, eventId, formatter(state, exception), fields.ToList()));
    }
}

// CurrentCulture flows with the async context, so it is pinned inside each test rather than per class.
internal sealed class CultureScope : IDisposable
{
    private readonly CultureInfo _previous;

    public CultureScope(string name)
    {
        _previous = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name);
    }

    public void Dispose() => CultureInfo.CurrentCulture = _previous;
}
