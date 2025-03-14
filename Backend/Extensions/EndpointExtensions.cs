namespace Laboratory.Backend.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
        => endpoints.MapGeneralEndpoints()
                    .MapAuthEndpoints()
                    .MapProductTypesEndpoints();

    public static IEndpointRouteBuilder MapGeneralEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/info", (HttpContext httpContext) =>
        {
            return Results.Ok(CreateContextOverview(httpContext));
        }).WithMetadata(AuthPermissions.Public);

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

    public static IEndpointRouteBuilder MapProductTypesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/ProductTypes",
            async (IProductTypesService service, CancellationToken cancellationToken)
                => (await service.GetAllAsync(cancellationToken)).Map())
            .WithMetadata(AuthPermissions.ProductTypes);

        endpoints.MapGet("/ProductTypes/{id}",
            async (IProductTypesService service, int id, CancellationToken cancellationToken)
                => (await service.GetByIdAsync(id, cancellationToken)).Map())
            .WithMetadata(AuthPermissions.ProductTypes);

        endpoints.MapPost("/ProductTypes",
            async (IProductTypesService service, [FromBody] ProductTypeDto @new, CancellationToken cancellationToken)
                => (await service.AddAsync(@new, cancellationToken)).Map())
            .WithMetadata(AuthPermissions.ProductTypesAdd);

        endpoints.MapPut("/ProductTypes/{id}",
            async (IProductTypesService service, int id, [FromBody] ProductTypeUpdateDto updated, CancellationToken cancellationToken)
                => (await service.UpdateAsync(id, updated, cancellationToken)).Map())
            .WithMetadata(AuthPermissions.ProductTypesEdit);

        return endpoints;
    }


}