using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace eShop.DiscountService;

public static class DiscountEndpoints
{
    public const string Route = "/api/discount/{id?}";

    public static IEndpointRouteBuilder MapDiscountApi(this IEndpointRouteBuilder app)
    {
        app.MapGet(Route, GetDiscount);
        app.MapPost(Route, PostDiscount);

        return app;
    }

    // The optional {id} segment mirrors the legacy Web API default route and is ignored.
    public static async Task<Results<Ok<DiscountResult>, BadRequest>> GetDiscount(
        [FromRoute] string? id,
        [FromQuery] string? customerId,
        [FromQuery] decimal amount,
        [FromQuery] int loyaltyYears,
        DiscountCalculator calculator,
        IOptions<DiscountOptions> options,
        ILogger logger,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            return TypedResults.BadRequest();
        }

        var result = calculator.Calculate(customerId, amount, loyaltyYears);

        await Task.Delay(options.Value.SimulatedMainframeDelay, ct);

        DiscountAuditLog.Write(logger, result);
        return TypedResults.Ok(result);
    }

    public static Task<Results<Ok<DiscountResult>, BadRequest>> PostDiscount(
        [FromRoute] string? id,
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] DiscountRequest? request,
        DiscountCalculator calculator,
        ILogger logger)
    {
        if (request is null)
        {
            return Task.FromResult<Results<Ok<DiscountResult>, BadRequest>>(TypedResults.BadRequest());
        }

        var result = calculator.Calculate(request.CustomerId, request.Amount, request.LoyaltyYears);

        DiscountAuditLog.Write(logger, result);
        return Task.FromResult<Results<Ok<DiscountResult>, BadRequest>>(TypedResults.Ok(result));
    }
}
