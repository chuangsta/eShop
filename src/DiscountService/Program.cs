using System.Text.Json.Serialization;
using eShop.DiscountService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<DiscountOptions>()
    .BindConfiguration(DiscountOptions.SectionName)
    .Validate(
        o => o.IsValid(),
        "Discount options are invalid: RoundingDigits must be 0..28, SimulatedMainframeDelay must not be negative and MaxPercent must be 0..100.")
    .ValidateOnStart();

builder.Services.AddSingleton<DiscountCalculator>();

// Handlers take a plain ILogger so the audit category stays stable and tests can pass any logger.
builder.Services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("eShop.DiscountService.DiscountAudit"));

// Newtonsoft accepted quoted numbers on read; keep that for existing callers.
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString);

var app = builder.Build();

app.MapDiscountApi();

app.Run();

public partial class Program { }
