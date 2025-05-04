namespace Laboratory.Backend.Interfaces;

public interface IResourcesService
{
    Task<ServiceResult<FileResult>> GetResourceAsync(string name, CancellationToken cancellationToken);
}
