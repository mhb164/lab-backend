namespace Shared.FileShare;

public class RenameFolderRequest
{
    public RenameFolderRequest() { /* Method intentionally left empty.*/ }

    public RenameFolderRequest(string? name, string? newName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{nameof(name)} is required!", nameof(name));
        
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException($"{nameof(newName)} is required!", nameof(newName));

        Name = name!.Trim();
        NewName = newName!.Trim();
    }

    public string? Name { get; set; }
    public string? NewName { get; set; }

    public ServiceResult Validate(Func<string?, string, ServiceResult> validateCharactersFunc)
    {
        if (string.IsNullOrWhiteSpace(Name))
            return ServiceResult.BadRequest($"{nameof(Name)} is required!");

        var nameCharacterValidationResult = validateCharactersFunc.Invoke(Name, nameof(Name));
        if (nameCharacterValidationResult.IsFailed)
            return nameCharacterValidationResult;

        if (string.IsNullOrWhiteSpace(NewName))
            return ServiceResult.BadRequest($"{nameof(NewName)} is required!");

        var newNameCharacterValidationResult = validateCharactersFunc.Invoke(NewName, nameof(NewName));
        if (newNameCharacterValidationResult.IsFailed)
            return newNameCharacterValidationResult;

        if (NewName == Name)
            return ServiceResult.BadRequest($"{nameof(NewName)} is the same as {nameof(Name)}!");

        return ServiceResult.Success();
    }
}
