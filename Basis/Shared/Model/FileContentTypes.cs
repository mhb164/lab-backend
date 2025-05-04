namespace Shared.Model;

public static class FileContentTypes
{
    // Text
    public static readonly string Plain = "text/plain";
    public static readonly string Csv = "text/csv";
    public static readonly string Html = "text/html";
    public static readonly string Css = "text/css";
    public static readonly string Xml = "application/xml";
    public static readonly string Json = "application/json";
    public static readonly string Js = "application/javascript";

    // Images
    public static readonly string Jpeg = "image/jpeg";
    public static readonly string Png = "image/png";
    public static readonly string Gif = "image/gif";
    public static readonly string Bmp = "image/bmp";
    public static readonly string Webp = "image/webp";
    public static readonly string Svg = "image/svg+xml";
    public static readonly string Icon = "image/x-icon";

    // Documents
    public static readonly string Pdf = "application/pdf";
    public static readonly string Doc = "application/msword";
    public static readonly string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    public static readonly string Xls = "application/vnd.ms-excel";
    public static readonly string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public static readonly string Ppt = "application/vnd.ms-powerpoint";
    public static readonly string Pptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

    // Archives
    public static readonly string Zip = "application/zip";
    public static readonly string Rar = "application/vnd.rar";
    public static readonly string SevenZip = "application/x-7z-compressed";
    public static readonly string Tar = "application/x-tar";
    public static readonly string Gzip = "application/gzip";

    // Audio
    public static readonly string Mp3 = "audio/mpeg";
    public static readonly string Wav = "audio/wav";
    public static readonly string Ogg = "audio/ogg";

    // Video
    public static readonly string Mp4 = "video/mp4";
    public static readonly string Avi = "video/x-msvideo";
    public static readonly string Mov = "video/quicktime";
    public static readonly string Wmv = "video/x-ms-wmv";

    // Default / Binary
    public static readonly string Binary = "application/octet-stream";
}
