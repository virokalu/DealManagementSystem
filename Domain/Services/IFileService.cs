using DealManagementSystem.Domain.Services.Communication;

namespace DealManagementSystem.Domain.Services;

public interface IFileService
{
    Task<Response<String>> SaveFileAsync(IFormFile? imageFile, string[] allowedFileExtensions);
    Task<Response<List<String>>> SaveFilesAsync(List<IFormFile>? mediaFiles, string[] allowedFileExtensions);
    void DeleteFile(string fileNameWithExtension);
}