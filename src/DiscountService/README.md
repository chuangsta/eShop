# DiscountService

ASP.NET Core 8 port of the legacy .NET Framework 4.8 Web API 2 discount
service. The original code is kept unchanged in `legacy/DiscountService` for
reference, and `tests/DiscountService.UnitTests` pins its behavior.

## Before and after

| Area | Before (`legacy/DiscountService`) | After (`src/DiscountService`) |
| --- | --- | --- |
| Hosting | System.Web on IIS (`Global.asax`, Web API 2 routing) | ASP.NET Core 8 minimal API (`MapDiscountApi()`) |
| Configuration | `web.config` appSettings | `appsettings.json` `Discount` section bound to `DiscountOptions` through `IOptions`, validated on start |
| Serialization | Newtonsoft.Json 9.0.1 | System.Text.Json (quoted numbers still accepted) |
| Audit | Pipe-delimited line appended to `App_Data/discount-audit.log` | Structured `ILogger` entry at Information level that renders the same audit line |
| Mainframe delay on GET | Blocking `Thread.Sleep(150)` | `await Task.Delay` (150 ms by default, configurable) |
| Tests | None | xUnit v3 characterization suite on Microsoft Testing Platform |

## Discount rules (unchanged)

- 1% per loyalty year.
- 5% extra when the customer id starts with `PREF` (case-sensitive).
- 10% extra when the order amount reaches the volume threshold. The
  threshold is inclusive: `amount >= 100` qualifies, so an order of exactly
  100 gets the bonus.
- The total percent is capped at a hard maximum of 20%.
- Only the discounted amount is rounded (2 digits, away from zero). The
  percent is never rounded.

## Build and test

```bash
dotnet build src/DiscountService/DiscountService.csproj
dotnet test --project tests/DiscountService.UnitTests/DiscountService.UnitTests.csproj
```
