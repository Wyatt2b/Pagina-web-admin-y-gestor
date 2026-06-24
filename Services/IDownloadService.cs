// Services/IDownloadService.cs
namespace AdminArboles.Services
{
    public interface IDownloadService
    {
        Task DownloadFileAsync(byte[] data, string fileName, string contentType);
    }
}