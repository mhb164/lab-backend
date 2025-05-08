using Common;
using System.IO;
using System.Xml.Linq;

namespace Laboratory.Backend.Services;

public class FileShareService : IFileShareService
{
    private static readonly HashSet<char> ExtraValidCharacters = new([' ', '_', '-', '.']);

    private readonly ILogger? _logger;
    private readonly FileShareConfig _config;

    public FileShareService(ILogger<FileShareService>? logger, FileShareConfig config)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<ServiceResult<FolderList>> GetFoldersAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var serviceResult = new ServiceResult<FolderList>();
        if (!_config.IsValid)
            return serviceResult.NoContent();

        return serviceResult.Success(CreateFolderList());
    }

    public async Task<ServiceResult<FolderList>> HandleAsync(CreateFolderRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var serviceResult = new ServiceResult<FolderList>();
        if (!_config.IsValid)
            return serviceResult.NoContent();

        var validationResult = request.Validate(ValidateCharacters);
        if (validationResult.IsFailed)
            return serviceResult.BadRequest(validationResult.Message!);

        var newDirectory = Path.Combine(_config.Directory, request.Name!);
        if (Directory.Exists(newDirectory))
            return serviceResult.Conflict($"{request.Name} already exists!");

        try
        {
            Directory.CreateDirectory(newDirectory);
            return serviceResult.Success(CreateFolderList());
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while create directory '{NewDirectory}'.", trackingId, newDirectory);
            return serviceResult.InternalError(trackingId, $"An error occured during create directory '{request.Name}'!");
        }
    }

    public async Task<ServiceResult<FolderList>> HandleAsync(RenameFolderRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var serviceResult = new ServiceResult<FolderList>();
        if (!_config.IsValid)
            return serviceResult.NoContent();

        var validationResult = request.Validate(ValidateCharacters);
        if (validationResult.IsFailed)
            return serviceResult.BadRequest(validationResult.Message!);

        var source = Path.Combine(_config.Directory, request.Name!);
        if (!Directory.Exists(source))
            return serviceResult.NotFound($"{request.Name} directory not found!");

        var destination = Path.Combine(_config.Directory, request.NewName!);
        if (Directory.Exists(destination))
            return serviceResult.Conflict($"{request.NewName} already exists!");

        try
        {
            Directory.Move(source, destination);
            return serviceResult.Success(CreateFolderList());
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while rename directory '{Source}' to '{Destination}'.", trackingId, source, destination);
            return serviceResult.InternalError(trackingId, $"An error occured during rename directory '{request.Name}' to {request.NewName}!");
        }
    }

    public async Task<ServiceResult<FolderList>> HandleAsync(DeleteFolderRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var serviceResult = new ServiceResult<FolderList>();
        if (!_config.IsValid)
            return serviceResult.NoContent();

        var validationResult = request.Validate(ValidateCharacters);
        if (validationResult.IsFailed)
            return serviceResult.BadRequest(validationResult.Message!);


        var directory = Path.Combine(_config.Directory, request.Name!);
        if (!Directory.Exists(directory))
            return serviceResult.NotFound($"{request.Name} directory not found!");

        try
        {
            Directory.Delete(directory, true);
            return serviceResult.Success(CreateFolderList());
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while delete directory '{Directory}'.", trackingId, directory);
            return serviceResult.InternalError(trackingId, $"An error occured during delete directory '{request.Name}'!");
        }
    }

    public async Task<ServiceResult<FolderFileList>> HandleAsync(FolderFilesRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var serviceResult = new ServiceResult<FolderFileList>();
        if (!_config.IsValid)
            return serviceResult.NoContent();

        var validationResult = request.Validate(ValidateCharacters);
        if (validationResult.IsFailed)
            return serviceResult.BadRequest(validationResult.Message!);

        var directory = Path.Combine(_config.Directory, request.Name!);
        if (!Directory.Exists(directory))
            return serviceResult.NotFound($"{request.Name} directory not found!");

        try
        {
            var files = new List<FolderFileListItem>();
            foreach (var item in Directory.EnumerateFiles(directory))
            {
                var file = new FileInfo(item);
                files.Add(new FolderFileListItem(file.Name, file.Length));
            }

            return serviceResult.Success(new FolderFileList(request.Name, files));
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while get directory files '{NewDirectory}'.", trackingId, directory);
            return serviceResult.InternalError(trackingId, $"An error occurred during get directory files '{request.Name}'!");
        }
    }

    public async Task<ServiceResult> UploadAsync(string folder, long length, string name, Func<Stream, CancellationToken, Task> copyToAsync, CancellationToken cancellationToken)
    {
        if (!_config.IsValid)
            return ServiceResult.NoContent();

        if (length <= 0)
            return ServiceResult.BadRequest($"No files uploaded.");

        var folderCharacterValidationResult = ValidateCharacters(folder, nameof(folder));
        if (folderCharacterValidationResult.IsFailed)
            return folderCharacterValidationResult;

        var nameCharacterValidationResult = ValidateCharacters(name, nameof(name));
        if (nameCharacterValidationResult.IsFailed)
            return nameCharacterValidationResult;

        var directory = Path.Combine(_config.Directory, folder);
        if (!Directory.Exists(directory))
            return ServiceResult.NotFound($"{folder} directory not found!");

        var filePath = Path.Combine(directory, Path.GetFileName(name));
        try
        {
            await using var stream = new FileStream(filePath, FileMode.Create);
            await copyToAsync(stream, cancellationToken);
            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while upload file to '{FilePath}'.", trackingId, filePath);
            return ServiceResult.InternalError(trackingId, $"An error occurred during upload '{name}'!");
        }
    }

    public async Task<ServiceResult<FileResult>> DownloadAsync(string folder, string name, CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<FileResult>();
        if (!_config.IsValid)
            return serviceResult.NoContent();

        var folderCharacterValidationResult = ValidateCharacters(folder, nameof(folder));
        if (folderCharacterValidationResult.IsFailed)
            return serviceResult.BadRequest(folderCharacterValidationResult.Message!);

        var nameCharacterValidationResult = ValidateCharacters(name, nameof(name));
        if (nameCharacterValidationResult.IsFailed)
            return serviceResult.BadRequest(nameCharacterValidationResult.Message!);

        var directory = Path.Combine(_config.Directory, folder);
        if (!Directory.Exists(directory))
            return serviceResult.NotFound($"{folder} directory not found!");

        var filePath = Path.Combine(directory, Path.GetFileName(name));
        try
        {
            var bytes = await File.ReadAllBytesAsync(filePath);
            var contentType = FileResult.GetContentType(name);

            return serviceResult.Success(new FileResult(contents: bytes, contentType: contentType, name: name));
        }
        catch (Exception ex)
        {
            var trackingId = Guid.NewGuid();
            _logger?.LogError(ex, "[{TrackingId}] An error occurred while download file from '{FilePath}'.", trackingId, filePath);
            return serviceResult.InternalError(trackingId, $"An error occurred during download file '{name}'!");
        }
    }


    private FolderList CreateFolderList()
    {
        var folders = new List<string>();
        foreach (var item in Directory.EnumerateDirectories(_config.Directory).Order())
        {
            folders.Add(Path.GetFileName(item));
        }

        return new FolderList(folders);
    }

    private static ServiceResult ValidateCharacters(string? content, string contentName)
    {
        if (string.IsNullOrWhiteSpace(content))
            return ServiceResult.BadRequest($"{contentName} is required!");

        foreach (var character in content)
        {
            if (!ValidateCharacter(character))
                return ServiceResult.BadRequest($"In {contentName} character '{character}' is invalid ('{content}')!");
        }

        if(content.EndsWith('.'))
            return ServiceResult.BadRequest($"{contentName} should not end with '.' ('{content}')!");

        return ServiceResult.Success();
    }

    private static bool ValidateCharacter(char character)
    {
        if (char.IsLetterOrDigit(character))
            return true;

        if (ExtraValidCharacters.Contains(character))
            return true;
        return false;
    }
}
