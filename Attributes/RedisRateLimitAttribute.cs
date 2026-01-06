namespace Dotnet_RateLimiter.Attributes;

public class RedisRateLimitAttribute : Attribute
{
    public int MaxRequests { get; }
    public int WindowSeconds { get; }
    
    public RedisRateLimitAttribute(int maxRequests, int windowSeconds)
    {
        MaxRequests = maxRequests;
        WindowSeconds = windowSeconds;
    }
}