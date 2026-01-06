using StackExchange.Redis;

namespace Dotnet_RateLimiter.Services;

public class RedisRateLimiter
{
    private readonly IDatabase _db;

    public RedisRateLimiter(IConnectionMultiplexer connectionMultiplexer)
    {
        _db = connectionMultiplexer.GetDatabase();
    }

    public async Task<bool> IsAllowedAsync(string key, int maxRequests, TimeSpan window)
    {
        var script = LuaScript.Prepare(@"
            local current = redis.call('INCR', @key)
            if tonumber(current) == 1 then
                redis.call('EXPIRE', @key, @window)
            end
            if tonumber(current) > tonumber(@maxRequests) then
                return 0
            else
                return 1
            end
        ");

        var result = await _db.ScriptEvaluateAsync(script, new
        {
            key = (RedisKey)$"rate_limit:{key}",
            window = (int)window.TotalSeconds,
            maxRequests = maxRequests
        });

        return (int)result == 1;
    }
}