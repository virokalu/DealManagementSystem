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

  public async Task<Response<List<String>>> SaveFilesAsync(List<IFormFile>? mediaFiles, string[] allowedFileExtensions)
  {
    if (mediaFiles == null || !mediaFiles.Any())
    {
      return new Response<List<String>>("Media not Found");
    }

    List<String> mediaList = new List<string>();

    try
    {
      var webRootPath = environment.WebRootPath;
      var path = Path.Combine(webRootPath, "Uploads");

      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }

      foreach (IFormFile file in mediaFiles)
      {
        // Check the allowed extenstions
        var ext = Path.GetExtension(file.FileName);
        if (!allowedFileExtensions.Contains(ext))
        {
          // throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
          return new Response<List<string>>($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
        }
        // generate a unique filename
        var fileName = $"{Guid.NewGuid()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);
        await using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await file.CopyToAsync(stream);
        mediaList.Add(fileName);
        
      }
      return new Response<List<String>>(item: mediaList);

    }
    catch (Exception e)
    {
      return new Response<List<String>>($"{e.Message}");
    }
  }

  public void DeleteFile(string fileNameWithExtension)
  {
    throw new NotImplementedException();
  }
}

