using DealManagementSystem.Domain.Services;
using DealManagementSystem.Domain.Services.Communication;

namespace DealManagementSystem.Services;

public class FileService(IWebHostEnvironment environment) : IFileService
{
  public async Task<Response<String>> SaveFileAsync(IFormFile? imageFile, string[] allowedFileExtensions)
  {
    if (imageFile == null)
    {
      return new Response<String>("Image not Found");
    }

    try
    {
      var webRootPath = environment.WebRootPath;
      var path = Path.Combine(webRootPath, "Uploads");

      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }

      // Check the allowed extenstions
      var ext = Path.GetExtension(imageFile.FileName);
      if (!allowedFileExtensions.Contains(ext))
      {
        // throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
        return new Response<string>($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
      }
      // generate a unique filename
      var fileName = $"{Guid.NewGuid()}{ext}";
      var fileNameWithPath = Path.Combine(path, fileName);
      await using var stream = new FileStream(fileNameWithPath, FileMode.Create);
      await imageFile.CopyToAsync(stream);
      return new Response<String>(item: fileName);

    }
    catch (Exception e)
    {
      return new Response<String>($"{e.Message}");
    }
  }
  public void DeleteFile(string fileNameWithExtension)
  {
    throw new NotImplementedException();
  }
}

