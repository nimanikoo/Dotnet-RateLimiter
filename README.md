

# Dotnet-RateLimiter

[![GitHub](https://img.shields.io/badge/Repository-GitHub-blue)](https://github.com/nimanikoo/Dotnet-RateLimiter)

**Dotnet-RateLimiter** is a .NET Core project designed to help control the rate of incoming requests to an API or resource. Implementing a rate limiter is crucial for preventing abuse, maintaining consistent performance, and ensuring fair usage of resources. This project showcases multiple types of rate limiting strategies, allowing you to choose the most suitable approach for your application’s needs.

## Features

This project includes the following rate limiting strategies:

1. **Fixed Window Rate Limiter**  
   A simple approach that limits the number of requests within a fixed time window. For example, with a limit of 100 requests per minute, all additional requests are blocked until the next minute starts. While easy to implement, this strategy may result in bursty behavior.

2. **Sliding Window Rate Limiter**  
   Similar to the fixed window approach, but with a continuously sliding window. This method ensures a more evenly distributed rate limit by allowing a rolling window of requests, reducing the impact of bursts.

3. **Token Bucket Rate Limiter**  
   A flexible rate limiter using the token bucket algorithm. Each request consumes a token, and tokens are replenished at a fixed rate over time. This approach is ideal for scenarios that require controlled bursts of requests, allowing clients to consume tokens quickly up to a defined limit.

4. **Concurrency Rate Limiter**  
   Controls the maximum number of simultaneous requests. For example, if the limit is set to 10, only 10 concurrent requests are allowed. As requests complete, new ones are allowed, maintaining the specified concurrency level.

## Getting Started

### Prerequisites

- .NET Core SDK 7.0+
- Visual Studio or any C# IDE

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/nimanikoo/Dotnet-RateLimiter.git
   cd dotnet-ratelimiter
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Run the project:

   ```bash
   dotnet run
   ```

### Usage

The project includes example implementations of each rate limiting strategy. You can explore the code and configure the rate limits, window sizes, token replenishment rates, and concurrency limits to fit your specific needs.

## Rate Limiting Strategies Explained

- **Fixed Window:** Best for simple scenarios where burstiness isn’t a major concern.
- **Sliding Window:** Provides more consistent rate enforcement, ideal for API endpoints with fluctuating traffic.
- **Token Bucket:** Offers flexibility with burst control while still maintaining a steady rate over time.
- **Concurrency Limiter:** Suitable when you need to limit simultaneous access to a resource, such as database connections or external API calls.

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.


