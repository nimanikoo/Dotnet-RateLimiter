using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Dotnet_RateLimiter.Extensions;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("FixedWindowPolicy", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(5);
                opt.PermitLimit = 5;
                opt.QueueLimit = 10;
                //LIFO approach : Last in First out 
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            }).RejectionStatusCode = 429;

            options.AddSlidingWindowLimiter("SlidingWindowPolicy", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(10);
                opt.PermitLimit = 5;
                opt.QueueLimit = 10;
                //LIFO approach : Last in First out
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.SegmentsPerWindow = 4;
            }).RejectionStatusCode = 429;

            options.AddConcurrencyLimiter("ConcurrencyPolicy", opt =>
            {
                opt.PermitLimit = 7;
                opt.QueueLimit = 3;
                //LIFO approach : Last in First out
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            }).RejectionStatusCode = 429;

            options.AddTokenBucketLimiter("BucketPolicy", opt =>
            {
                opt.TokenLimit = 5;
                opt.QueueLimit = 2;
                //LIFO approach : Last in First out
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
                opt.AutoReplenishment = true;
                opt.TokensPerPeriod = 3;
            }).RejectionStatusCode = 429;
        });
        return services;
    }
}