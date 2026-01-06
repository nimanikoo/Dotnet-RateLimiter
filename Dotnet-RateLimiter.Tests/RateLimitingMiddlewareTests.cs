using System.Security.Claims;
using Dotnet_RateLimiter.Attributes;
using Dotnet_RateLimiter.Middlewares;
using Dotnet_RateLimiter.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using StackExchange.Redis;

namespace Dotnet_RateLimiter.Tests;

public class RateLimitingMiddlewareTests
{
    private readonly Mock<RedisRateLimiter> _mockLimiter;
    private readonly RequestDelegate _next = (innerHttpContext) => Task.CompletedTask;

    public RateLimitingMiddlewareTests()
    {
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();
        _mockLimiter = new Mock<RedisRateLimiter>(mockMultiplexer.Object);
    }

    [Fact]
    public async Task InvokeAsync_AuthenticatedUser_UsesUserIdInKey()
    {
        // Arrange
        var context = new DefaultHttpContext();

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "user-123") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        context.User = new ClaimsPrincipal(identity);

        var endpoint = new Endpoint(null, new EndpointMetadataCollection(new RedisRateLimitAttribute(10, 60)), "Test");
        context.SetEndpoint(endpoint);

        var middleware = new RedisRateLimitingMiddleware(_next, _mockLimiter.Object);

        _mockLimiter.Setup(l => l.IsAllowedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(true);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _mockLimiter.Verify(l => l.IsAllowedAsync(
                It.Is<string>(s => s.Contains("user:user-123")),
                It.IsAny<int>(),
                It.IsAny<TimeSpan>()),
            Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenRateLimitExceeded_Returns429()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");

        var endpoint = new Endpoint(null, new EndpointMetadataCollection(new RedisRateLimitAttribute(5, 60)), "Test");
        context.SetEndpoint(endpoint);

        var middleware = new RedisRateLimitingMiddleware(_next, _mockLimiter.Object);

        _mockLimiter.Setup(l => l.IsAllowedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(false);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
    }
}