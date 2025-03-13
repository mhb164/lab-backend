namespace Laboratory.Backend.Extensions;

public static class EndpointExtensions
{
    private static IResult Map<T>(ServiceResult<T> result) => result.Code switch
    {
        ServiceResultCode.Success => Results.Ok(result.Value),
        _ => ErrorResponse.Generate(result),
    };

    private static IResult Map(ServiceResult result) => result.Code switch
    {
        ServiceResultCode.Success => Results.Ok(),
        _ => ErrorResponse.Generate(result),
    };

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
        => endpoints.MapGeneralEndpoints()
                    .MapAuthEndpoints()
                    .MapProductTypesEndpoints();

    public static IEndpointRouteBuilder MapGeneralEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/info", (HttpContext httpContext) =>
        {
            return Results.Ok(CreateContextOverview(httpContext));
        }).WithMetadata(ApiPermissions.Public);

        return endpoints;
    }

    private static ContextOverview CreateContextOverview(HttpContext httpContext)
    {
        var clientUser = httpContext?.GetClientUser();
        var contextOverview = new ContextOverview(Aid.ProductName, Aid.AppInformationalVersion,
            clientIpAddress: httpContext?.Connection?.RemoteIpAddress?.ToString(),
            authenticated: clientUser != null);

        return contextOverview;
    }

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/sign-in",
            async ([FromBody] SignInRequest request, IAuthService service, CancellationToken cancellationToken)
                => Map(await service.SignInAsync(request, cancellationToken)))
            .WithMetadata(ApiPermissions.Public);

        endpoints.MapPost("/change-local-password",
           async ([FromBody] ChangeLocalPasswordRequest request, IAuthService service, CancellationToken cancellationToken)
               => Map(await service.ChangeLocalPasswordRequestAsync(request, cancellationToken)))
           .WithMetadata(ApiPermissions.TokenIsEnough);

        endpoints.MapPost("/refresh-token",
            async (HttpContext httpContext, IAuthService service, CancellationToken cancellationToken)
                => Map(await service.RefreshTokenAsync(httpContext.GetRefreshToken(), cancellationToken)))
            .WithMetadata(ApiPermissions.Public);

        endpoints.MapPost("/sign-out",
            async (IAuthService service, CancellationToken cancellationToken)
                => Map(await service.SignOutAsync(cancellationToken)))
            .WithMetadata(ApiPermissions.TokenIsEnough);

        return endpoints;
    }

    public static IEndpointRouteBuilder MapProductTypesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/ProductTypes",
            async (IProductTypesService service, CancellationToken cancellationToken)
                => Map(await service.GetAllAsync(cancellationToken)))
            .WithMetadata(ApiPermissions.ProductTypes);

        endpoints.MapGet("/ProductTypes/{id}",
            async (IProductTypesService service, int id, CancellationToken cancellationToken)
                => Map(await service.GetByIdAsync(id, cancellationToken)))
            .WithMetadata(ApiPermissions.ProductTypes);

        endpoints.MapPost("/ProductTypes",
            async (IProductTypesService service, [FromBody] ProductTypeDto @new, CancellationToken cancellationToken)
                => Map(await service.AddAsync(@new, cancellationToken)))
            .WithMetadata(ApiPermissions.ProductTypesAdd);

        endpoints.MapPut("/ProductTypes/{id}",
            async (IProductTypesService service, int id, [FromBody] ProductTypeUpdateDto updated, CancellationToken cancellationToken)
                => Map(await service.UpdateAsync(id, updated, cancellationToken)))
            .WithMetadata(ApiPermissions.ProductTypesEdit);

        return endpoints;
    }


}