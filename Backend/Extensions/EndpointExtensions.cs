using Shared.FileShare;
using System.Threading;

namespace Laboratory.Backend.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
        => endpoints.MapGeneralEndpoints()
                    .MapAuthEndpoints()
                    .MapResourcesEndpoints()
                    .MapFileShareEndpoints()
                    .MapProductTypesEndpoints();

    public static IEndpointRouteBuilder MapGeneralEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/info", (HttpContext httpContext) =>
        {
            return Results.Ok(CreateContextOverview(httpContext));
        }).WithMetadata(AuthPermissions.Public);

        endpoints.MapPost("/crypto", async (EncryptionService service, CryptoRequest request) =>
        {
            var serviceResult = new ServiceResult<string>();
            if (request is null)
                return serviceResult.BadRequest("request is required!").Map();

            var validateResult = request.Validate();
            if (validateResult.IsFailed)
                return serviceResult.BadRequest(validateResult.Message!).Map();

            try
            {
                var result = string.Empty;
                switch (request.Operation!)
                {
                    case CryptoOperation.Encrypt:
                        result = await service.EncryptAsync(request.Content!, request.Password!, request.Iterations!.Value);
                        break;
                    case CryptoOperation.Decrypt:
                        result = await service.DecryptAsync(request.Content!, request.Password!, request.Iterations!.Value);
                        break;
                }

                return serviceResult.Success(result).Map();
            }
            catch (Exception ex)
            {
                return serviceResult.BadRequest(ex.Message!).Map();
            }

        }).WithMetadata(AuthPermissions.Public);

        return endpoints;
    }


    private static ContextOverview CreateContextOverview(HttpContext httpContext)
    {
        var clientUser = httpContext?.GetClientUser();
        var contextOverview = new ContextOverview(Aid.AppInfo.ProductName, Aid.AppInfo.InformationalVersion,
            clientIpAddress: httpContext?.Connection?.RemoteIpAddress?.ToString(),
            authenticated: clientUser != null);

        return contextOverview;
    }

    public static IEndpointRouteBuilder MapResourcesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/resources/{name}",
            async (IResourcesService service, string name, CancellationToken cancellationToken)
                => await service.GetResourceAsync(name, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);

        endpoints.MapPost("/resources/{name}",
            async (IResourcesService service, string name, CancellationToken cancellationToken)
                => await service.GetResourceAsync(name, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);
        return endpoints;
    }

    public static IEndpointRouteBuilder MapFileShareEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/share/folders",
            async (IFileShareService service, BlankRequest request, CancellationToken cancellationToken)
                => await service.GetFoldersAsync(cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);

        endpoints.MapPost("/share/create-folder",
            async (IFileShareService service, CreateFolderRequest request, CancellationToken cancellationToken)
                => await service.HandleAsync(request, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);

        endpoints.MapPost("/share/rename-folder",
            async (IFileShareService service, RenameFolderRequest request, CancellationToken cancellationToken)
                => await service.HandleAsync(request, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);

        endpoints.MapPost("/share/delete-folder",
            async (IFileShareService service, DeleteFolderRequest request, CancellationToken cancellationToken)
                => await service.HandleAsync(request, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);

        endpoints.MapPost("/share/files",
            async (IFileShareService service, FolderFilesRequest request, CancellationToken cancellationToken)
                => await service.HandleAsync(request, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);

        endpoints.MapPost("/share/upload/{folder}",
            async (IFileShareService service, string folder, IFormFile files, CancellationToken cancellationToken)
                => await service.UploadAsync(folder, files.Length, files.FileName, files.CopyToAsync, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough)
            .DisableAntiforgery();

        endpoints.MapPost("/share/download/{folder}/{name}",
            async (IFileShareService service, string folder, string name, CancellationToken cancellationToken)
                => await service.DownloadAsync(folder, name, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.TokenIsEnough);

        return endpoints;
    }

    public static IEndpointRouteBuilder MapProductTypesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/ProductTypes",
            async (IProductTypesService service, CancellationToken cancellationToken)
               => await service.GetAllAsync(cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.ProductTypes);

        endpoints.MapGet("/ProductTypes/{id}",
            async (IProductTypesService service, int id, CancellationToken cancellationToken)
                => await service.GetByIdAsync(id, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.ProductTypes);

        endpoints.MapPost("/ProductTypes",
            async (IProductTypesService service, [FromBody] ProductTypeDto @new, CancellationToken cancellationToken)
                => await service.AddAsync(@new, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.ProductTypesAdd);

        endpoints.MapPut("/ProductTypes/{id}",
            async (IProductTypesService service, int id, [FromBody] ProductTypeUpdateDto updated, CancellationToken cancellationToken)
                => await service.UpdateAsync(id, updated, cancellationToken).MapAsync())
            .WithMetadata(AuthPermissions.ProductTypesEdit);

        return endpoints;
    }


}