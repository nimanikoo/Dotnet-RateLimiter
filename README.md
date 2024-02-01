# Dotnet-RateLimiter
This is a rate limiter project implemented using .NET Core. The rate limiter helps control the number of requests made to an API or a resource within a specified time frame.
In .NET Core, there are various types of rate limiters you can implement, depending on your specific requirements and the level of control you need over the rate limiting logic. Here are a few common types of rate limiters used in .NET Core:

## **Fixed Window Rate Limiter:**
This type of rate limiter allows a fixed number of requests within a specific time window. For example, you can enforce a rate limit of 100 requests per minute. Once the limit is reached, further requests are rejected until the window resets. This approach is simple to implement but can result in bursty behavior if requests are made in quick succession.

## **Sliding Window Rate Limiter:**
A sliding window rate limiter also enforces a fixed number of requests within a time window, but the window slides continuously rather than resetting at fixed intervals. For example, if you have a rate limit of 100 requests per minute, the sliding window rate limiter allows 100 requests in any rolling 60-second window. This approach provides a more evenly distributed rate limit and handles bursts of requests more effectively.

## **Token Bucket Rate Limiter:** 
The token bucket algorithm is a popular approach for rate limiting. In this approach, clients are assigned a fixed number of tokens. Each request consumes a token, and requests can only be processed if tokens are available. Tokens are replenished at a certain rate over time. This allows for more flexible rate limiting scenarios where clients can burst requests up to a certain token limit. The "TokenBucket" algorithm is commonly used for token-based rate limiting.

## **Concurrency Rate Limiter:**
A concurrency limiter controls the maximum number of simultaneous requests to a resource. If you set a limit of 10, for example, only the first 10 requests will be granted access to the resource at a given point of time. Whenever a request completes, it opens a slot for a new request.

## **Contributing:**
Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.
For any questions or inquiries, please contact **nikoonazar.nima@gmail.com**
