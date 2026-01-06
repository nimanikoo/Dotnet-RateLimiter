using System.Security.Claims;
using Dotnet_RateLimiter.Attributes;
using Dotnet_RateLimiter.Services;

namespace Dotnet_RateLimiter.Middlewares;

public class RedisRateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RedisRateLimiter _rateLimiter;

    public RedisRateLimitingMiddleware(RequestDelegate next, RedisRateLimiter rateLimiter)
    {
        _next = next;
        _rateLimiter = rateLimiter;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        var rateLimitAttr = endpoint.Metadata.GetMetadata<RedisRateLimitAttribute>();
        if (rateLimitAttr == null)
        {
            await _next(context);
            return;
        }

        string identityKey;
        string userType;

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? context.User.Identity.Name
                         ?? "authenticated_unknown";

            identityKey = userId;
            userType = "user";
        }
        else
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            identityKey = ip;
            userType = "guest";
        }
        
        var limitKey = $"{userType}:{identityKey}:{context.Request.Path}";

        var isAllowed = await _rateLimiter.IsAllowedAsync(
            limitKey,
            rateLimitAttr.MaxRequests,
            TimeSpan.FromSeconds(rateLimitAttr.WindowSeconds)
        );

        if (!isAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers.RetryAfter = rateLimitAttr.WindowSeconds.ToString();

            var message = userType == "user"
                ? "Dear user, you've reached your limit. Take a breath!"
                : "Guest limit reached. Please sign up for more quota!";

            await context.Response.WriteAsync(message);
            return;
        }

        await _next(context);
    }
}