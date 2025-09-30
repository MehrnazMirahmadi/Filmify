using System.Collections.Concurrent;
using System.Net;

namespace Filmify.Api.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly ConcurrentDictionary<string, ClientInfo> _clients = new();
    private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);
    private readonly int _maxRequests = 100; // Max requests per minute per IP

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = GetClientIpAddress(context);
        var now = DateTime.UtcNow;

        var clientInfo = _clients.AddOrUpdate(clientIp, 
            new ClientInfo { RequestCount = 1, FirstRequestTime = now },
            (key, existing) =>
            {
                if (now - existing.FirstRequestTime > _timeWindow)
                {
                    return new ClientInfo { RequestCount = 1, FirstRequestTime = now };
                }
                return new ClientInfo 
                { 
                    RequestCount = existing.RequestCount + 1, 
                    FirstRequestTime = existing.FirstRequestTime 
                };
            });

        if (clientInfo.RequestCount > _maxRequests)
        {
            _logger.LogWarning("Rate limit exceeded for IP: {ClientIp}. Requests: {RequestCount}", 
                clientIp, clientInfo.RequestCount);
            
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
            return;
        }

        await _next(context);
    }

    private static string GetClientIpAddress(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        
        // Check for forwarded IP (behind proxy/load balancer)
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim();
        }
        else if (context.Request.Headers.ContainsKey("X-Real-IP"))
        {
            ipAddress = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        }

        return ipAddress ?? "unknown";
    }

    private class ClientInfo
    {
        public int RequestCount { get; set; }
        public DateTime FirstRequestTime { get; set; }
    }
}
