namespace Laboratory.Backend.Services;

public class ResourcesService : IResourcesService
{
    private readonly ILogger? _logger;
    private readonly ResourcesConfig _config;

    public ResourcesService(ILogger<ResourcesService>? logger, ResourcesConfig config)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }


    public async Task<ServiceResult<FileResult>> GetResourceAsync(string name, CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<FileResult>();

        if (!_config.IsValid)
            return serviceResult.NoContent();

        var filename = Directory.GetFiles(_config.Directory!, name).FirstOrDefault();

        if (filename is null)
            return serviceResult.NotFound($"{name} not found!");

        var bytes = await File.ReadAllBytesAsync(filename);
        var contentType = FileResult.GetContentType(name);

        return serviceResult.Success(new FileResult(contents: bytes, contentType: contentType, name: name));
    }
}
