namespace Shared.FileShare;

public class DeleteFolderRequest
{
    public DeleteFolderRequest() { /* Method intentionally left empty.*/ }

    public DeleteFolderRequest(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{nameof(name)} is required!", nameof(name));

        Name = name!.Trim();
    }

    public string? Name { get; set; }

    public ServiceResult Validate(Func<string?, string, ServiceResult> validateCharactersFunc)
    {
        if (string.IsNullOrWhiteSpace(Name))
            return ServiceResult.BadRequest($"{nameof(Name)} is required!");

        var nameCharacterValidationResult = validateCharactersFunc.Invoke(Name, nameof(Name));
        if (nameCharacterValidationResult.IsFailed)
            return nameCharacterValidationResult;

        return ServiceResult.Success();
    }
}