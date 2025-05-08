namespace Shared.FileShare;

public class FolderList
{
    public FolderList() { /* Method intentionally left empty.*/ }

    public FolderList(List<string>? items)
    {
        Items = items ?? new List<string>();
    }

    public List<string> Items { get; set; }
}
