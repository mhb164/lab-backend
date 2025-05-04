namespace Shared.Extensions;

public static class IResultExtensions
{
    public static IResult Map<T>(this ServiceResult<T> result) => result.Code switch
    {
        ServiceResultCode.Success => OK(result.Value),
        _ => ErrorResponse.Generate(result),
    };
    
    public static IResult Map(this ServiceResult result) => result.Code switch
    {
        ServiceResultCode.Success => Results.Ok(),
        _ => ErrorResponse.Generate(result),
    };

    public static async Task<IResult> MapAsync<T>(this Task<ServiceResult<T>> task)
        => (await task).Map();

    public static async Task<IResult> MapAsync(this Task<ServiceResult> task)
        => (await task).Map();

    private static IResult OK<T>(T? value)
    {
        if (value is FileResult fileResult)
            return Results.File(fileResult.Contents, fileResult.ContentType, fileResult.Name);

        return Results.Ok(value);
    }

}
