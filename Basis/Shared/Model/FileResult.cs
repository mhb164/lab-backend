namespace Shared.Model;

public sealed class FileResult
{
    public FileResult() { /* Method intentionally left empty.*/ }

    public FileResult(string contents, string? name, string? contentType)
        : this(Encoding.UTF8.GetBytes(contents), name, contentType) { }

    public FileResult(byte[] contents, string? name, string? contentType)
    {
        Contents = contents;
        Name = name;
        ContentType = contentType;
    }

    public byte[] Contents { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }

    public static string GetContentType(string name)
    {
        var extension = Path.GetExtension(name).ToLowerInvariant();

        return extension switch
        {
            ".txt" => "text/plain",
            ".html" => "text/html",
            ".htm" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".csv" => "text/csv",

            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            ".ico" => "image/x-icon",

            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",

            ".zip" => "application/zip",
            ".rar" => "application/vnd.rar",
            ".7z" => "application/x-7z-compressed",
            ".tar" => "application/x-tar",
            ".gz" => "application/gzip",

            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",

            ".mp4" => "video/mp4",
            ".avi" => "video/x-msvideo",
            ".mov" => "video/quicktime",
            ".wmv" => "video/x-ms-wmv",

            _ => "application/octet-stream", // default binary content type
        };
    }
}
