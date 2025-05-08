namespace Shared.FileShare;

public class FolderFileList
{
    public FolderFileList() { /* Method intentionally left empty.*/ }

    public FolderFileList(string? name, List<FolderFileListItem>? items)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{nameof(name)} is required!", nameof(name));

        Name = name!.Trim();
        Items = items ?? new List<FolderFileListItem>();
    }

    public string? Name { get; set; }
    public List<FolderFileListItem> Items { get; set; }
}
