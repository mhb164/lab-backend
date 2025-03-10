namespace Backend;

public class ServiceResult
{
    public readonly Guid? TrackingId;
    public readonly string? Message;
    public readonly ServiceResultCode Code;

    protected ServiceResult(ServiceResultCode code, string message)
    {
        Code = code;
        Message = message;
    }

    protected ServiceResult(Guid trackingId, ServiceResultCode code, string message)
    {
        TrackingId = trackingId;
        Code = code;
        Message = message;
    }

    public bool IsSuccess => Code == ServiceResultCode.Success;
    public bool IsFailed => Code != ServiceResultCode.Success;

    public static ServiceResult Success() => new(ServiceResultCode.Success, string.Empty);
    public static ServiceResult<T> Success<T>(T value) => new ServiceResult<T>().Success(value);

    public static ServiceResult InternalError(Guid trackingId, string message) => new(trackingId, ServiceResultCode.InternalError, message);
    public static ServiceResult<T> InternalError<T>(Guid trackingId, string message) => new ServiceResult<T>().InternalError(trackingId, message);

    private static ServiceResult Failed(ServiceResultCode code, string message) => new(code, message);

    public static ServiceResult BadRequest(string message) => Failed(ServiceResultCode.BadRequest, message);
    public static ServiceResult<T> BadRequest<T>(string message) => new ServiceResult<T>().BadRequest(message);

    public static ServiceResult Unauthorized(string message) => Failed(ServiceResultCode.Unauthorized, message);
    public static ServiceResult<T> Unauthorized<T>(string message) => new ServiceResult<T>().Unauthorized(message);

    public static ServiceResult Forbidden(string message) => Failed(ServiceResultCode.Forbidden, message);
    public static ServiceResult<T> Forbidden<T>(string message) => new ServiceResult<T>().Forbidden(message);

    public static ServiceResult NotFound(string message) => Failed(ServiceResultCode.NotFound, message);
    public static ServiceResult<T> NotFound<T>(string message) => new ServiceResult<T>().NotFound(message);

    public static ServiceResult Conflict(string message) => Failed(ServiceResultCode.Conflict, message);
    public static ServiceResult<T> Conflict<T>(string message) => new ServiceResult<T>().Conflict(message);

    public static ServiceResult NotImplemented(string message) => Failed(ServiceResultCode.NotImplemented, message);
    public static ServiceResult<T> NotImplemented<T>(string message) => new ServiceResult<T>().NotImplemented(message);

    public static ServiceResult ServiceUnavailable(string message) => Failed(ServiceResultCode.ServiceUnavailable, message);
    public static ServiceResult<T> ServiceUnavailable<T>(string message) => new ServiceResult<T>().ServiceUnavailable(message);
}
