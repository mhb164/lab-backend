namespace Shared.Middleware;

public class ExceptionMiddleware
{
    private readonly Microsoft.Extensions.Logging.ILogger? _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware>? logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        try
        {
            await _next(context); // Process request
        }
        catch (Exception ex)
        {
            var errorResponse = ErrorResponse.Generate(ex);
            _logger?.LogError(ex, "[{TrackingId}] An unhandled exception occurred.", errorResponse.TrackingId);

            await errorResponse.ExecuteAsync(context);
        }
    }
}
