namespace Shared.FileShare;

public class FolderFileListItem
{
    public FolderFileListItem() { /* Method intentionally left empty.*/ }

    public FolderFileListItem(string? name, long length)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{nameof(name)} is required!", nameof(name));

        Name = name!.Trim();
        Length = length;
    }

    public string? Name { get; set; }
    public long? Length { get; set; }
}
