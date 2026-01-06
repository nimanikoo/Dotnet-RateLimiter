# 🚀 Distributed Rate Limiter: The High-Performance Suite for .NET 10

[![.NET 10](https://img.shields.io/badge/.NET-10.0-blueviolet?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Redis](https://img.shields.io/badge/Redis-Latest-red?style=for-the-badge&logo=redis)](https://redis.io/)
[![Lua](https://img.shields.io/badge/Logic-Lua_Scripting-blue?style=for-the-badge&logo=lua)](https://lua.org/)
[![Docker](https://img.shields.io/badge/Docker-Orchestrated-blue?style=for-the-badge&logo=docker)](https://www.docker.com/)

**Dotnet-RateLimiter** is not just a simple throttling tool; it is a **production-ready distributed infrastructure** designed to handle high-traffic environments. By combining the cutting-edge features of **.NET 10** with the raw power of **Redis Lua Scripting**, this project solves the most critical challenges of rate limiting in microservices: **Race Conditions** and **Consistency**.

---

## 💎 The Engineering Excellence: Why Redis + Lua?

In a distributed environment (multiple API instances), standard in-memory limiting fails because each server has its own state. Our solution moves the "Source of Truth" to Redis, optimized by server-side Lua execution.

### ⚡ Atomic "Read-Modify-Write"
Most distributed limiters suffer from **Race Conditions**. If two requests hit two different servers at the exact same millisecond, they might both read a counter of `9` (limit `10`) and both proceed, incorrectly allowing 11 requests.
**Our Solution:** We offload the entire logic to a **Lua Script** executed inside Redis. Redis guarantees that the script runs as a single atomic operation. No other command can run until the logic is finished. **Consistency is 100% guaranteed.**

### 🧠 Identity-Aware Logic
The system intelligently resolves client identities to ensure fair usage:
- **Authenticated Users:** Automatically tracks limits via `User.Identity` (Claims-based).
- **Anonymous Guests:** Falls back to `Client IP` resolution.
- **Granular Scoping:** Limits are unique per `User/IP + API Path`, ensuring one heavy endpoint doesn't block the rest of the application.

---

## 🛠 Infrastructure & Observability

We believe that a professional system must be "Observable." This project includes a full monitoring stack out of the box.

### 🔍 Advanced Health Checks UI
Located at `/health-ui`, our dashboard provides a real-time status of the system's vital signs:
- **Redis Connectivity:** Real-time latency and connection status.
- **System Liveness:** Ensures the .NET runtime is healthy.
- **Fail-Fast Mechanism:** Integrated circuit-breaker logic for infrastructure dependencies.

### 📡 Live Data Inspection (RedisInsight)
The environment is pre-configured with **RedisInsight** (Port `8001`). You can visually monitor:
- Rate-limit keys being created in real-time.
- Decrementing TTLs (Time-To-Live).
- Counter values managed by the Lua engine.

---

## 🏗 Supported Strategies

While the **Distributed Redis Limiter** is the flagship feature, the project supports a variety of patterns:
1. **Distributed Fixed Window (Redis + Lua):** Atomic, cluster-wide throttling.
2. **Standard Fixed Window:** Fast, in-memory limiting for single-node apps.
3. **Sliding Window:** Smooths out traffic spikes by using rolling timeframes.
4. **Token Bucket:** Allows for controlled "burstiness" for better UX.
5. **Concurrency Limiter:** Protects downstream resources (DBs/External APIs) from being overwhelmed.

---

## 🚀 One-Command Deployment

Experience the full stack (API, Redis, and Monitoring) immediately using Docker Compose:

```bash
docker-compose up --build -d
```

### Key Endpoints:
- 🌐 **API Gateway:** `http://localhost:8080`
- 🩺 **Health Dashboard:** `http://localhost:8080/health-ui`
- 💎 **Redis Insight UI:** `http://localhost:8001` (Host: `redis` | Port: `6379`)

---

## 💻 Seamless Integration

Implementing high-performance throttling is as simple as adding a single attribute:

```csharp
[ApiController]
[Route("api/secure")]
public class DataController : ControllerBase
{
    // Allows 10 requests every 60 seconds per Identity/IP
    [RedisRateLimit(maxRequests: 10, windowSeconds: 60)]
    [HttpGet]
    public IActionResult Get() => Ok("Protected by Atomic Redis Logic.");
}
```

---

## 🧪 Scalable Testing
The project includes a dedicated **xUnit** suite. We focus on:
- **Middleware Pipeline:** Testing identity resolution.
- **Key Generation:** Ensuring unique keys for different users/paths.
- **Atomic Mocking:** Simulating Redis failures to test system resilience.

```bash
dotnet test
```

---

## 🤝 Contributing
Built for the community by developers who care about performance. Feel free to open an issue or submit a PR to improve the Lua scripts or add new strategies.

---
**Created with ❤️ by [Nima Nikoo](https://github.com/nimanikoo)**
