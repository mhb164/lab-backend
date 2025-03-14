
namespace Shared;

public class ErrorResponse : IResult
{
    public ErrorResponse(int statusCode, string? message)
    {
        Time = DateTime.Now;
        StatusCode = statusCode;
        Message = message;
    }

    public ErrorResponse(Guid? trackingId, int statusCode, string? message)
    {
        Time = DateTime.Now;
        TrackingId = trackingId;
        StatusCode = statusCode;
        Message = message;
    }

    public int StatusCode { get; }
    public DateTime Time { get; }
    public Guid? TrackingId { get; }
    public string? Message { get; }

    public static ErrorResponse Generate(Exception exception)
    {
        if (exception is UnauthorizedAccessException)
            return new ErrorResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.");

        if (exception is KeyNotFoundException)
            return new ErrorResponse((int)HttpStatusCode.NotFound, "Resource not found.");

        return new ErrorResponse(Guid.NewGuid(), (int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please check by tracking-Id.");
    }

    public static ErrorResponse Generate<T>(ServiceResult<T> result)
    {
        if (result.Code == ServiceResultCode.InternalError)
            return new ErrorResponse(result.TrackingId, (int)HttpStatusCode.InternalServerError, $"{result}. Please check by tracking-Id.");

        return new ErrorResponse((int)result.Code, result.Message);
    }

    public static ErrorResponse Generate(ServiceResult result)
    {
        if (result.Code == ServiceResultCode.InternalError)
            return new ErrorResponse(result.TrackingId, (int)HttpStatusCode.InternalServerError, $"{result}. Please check by tracking-Id.");

        return new ErrorResponse((int)result.Code, result.Message);
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCode;
        var json = JsonSerializer.Serialize(this);

        return httpContext.Response.WriteAsync(json);
    }
}
