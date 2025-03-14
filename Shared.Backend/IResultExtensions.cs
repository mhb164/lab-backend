namespace Shared;

public static class IResultExtensions
{
    public static IResult Map<T>(this ServiceResult<T> result) => result.Code switch
    {
        ServiceResultCode.Success => Results.Ok(result.Value),
        _ => ErrorResponse.Generate(result),
    };

    public static IResult Map(this ServiceResult result) => result.Code switch
    {
        ServiceResultCode.Success => Results.Ok(),
        _ => ErrorResponse.Generate(result),
    };
}
