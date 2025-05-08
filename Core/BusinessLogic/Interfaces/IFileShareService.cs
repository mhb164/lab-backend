
namespace Laboratory.Backend.Interfaces;

public interface IFileShareService
{
    Task<ServiceResult<FolderList>> GetFoldersAsync(CancellationToken cancellationToken);
    Task<ServiceResult<FolderList>> HandleAsync(CreateFolderRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<FolderList>> HandleAsync(RenameFolderRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<FolderList>> HandleAsync(DeleteFolderRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<FolderFileList>> HandleAsync(FolderFilesRequest request, CancellationToken cancellationToken);
    Task<ServiceResult> UploadAsync(string folder, long length, string name, Func<Stream, CancellationToken, Task> copyToAsync, CancellationToken cancellationToken);
    Task<ServiceResult<FileResult>> HandleAsync(DownloadFileRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<FolderFileList>> HandleAsync(DeleteFileRequest request, CancellationToken cancellationToken);
}
