namespace Shared.FileShare;

public class DownloadFileRequest
{
    public DownloadFileRequest() { /* Method intentionally left empty.*/ }

    public DownloadFileRequest(string? folder, string? name)
    {
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException($"{nameof(folder)} is required!", nameof(folder));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{nameof(name)} is required!", nameof(name));

        Folder = folder!.Trim();
        Name = name!.Trim();
    }

    public string? Folder { get; set; }
    public string? Name { get; set; }

    public ServiceResult Validate(Func<string?, string, ServiceResult> validateCharactersFunc)
    {
        if (string.IsNullOrWhiteSpace(Folder))
            return ServiceResult.BadRequest($"{nameof(Folder)} is required!");

        var folderCharacterValidationResult = validateCharactersFunc.Invoke(Folder, nameof(Folder));
        if (folderCharacterValidationResult.IsFailed)
            return folderCharacterValidationResult;

        if (string.IsNullOrWhiteSpace(Name))
            return ServiceResult.BadRequest($"{nameof(Name)} is required!");

        var nameCharacterValidationResult = validateCharactersFunc.Invoke(Name, nameof(Name));
        if (nameCharacterValidationResult.IsFailed)
            return nameCharacterValidationResult;

        return ServiceResult.Success();
    }
}
