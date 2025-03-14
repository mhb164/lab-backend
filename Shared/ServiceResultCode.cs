namespace Shared;

public enum ServiceResultCode
{
    Unknown = 0,

    Success = 200,
    NoContent = 204,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    InternalError = 500,
    NotImplemented = 501, //Status501NotImplemented
    ServiceUnavailable = 503, //Status503ServiceUnavailable
}
