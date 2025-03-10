namespace Backend;

public enum ServiceResultCode
{
    Success = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    InternalError = 500,
    NotImplemented = 501, //Status501NotImplemented
    ServiceUnavailable = 503, //Status503ServiceUnavailable
}
