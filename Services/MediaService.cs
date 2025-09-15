using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services;
using DealManagementSystem.Domain.Services.Communication;
using DealManagementSystem.Persistence.Context;

namespace DealManagementSystem.Services;

public class MediaService : IMediaService
{
    private readonly DealContext _context;
    private readonly IFileService _fileService;
    private readonly string[] _allowedMediaExtentions = [".mp4", ".avi", ".mov", ".webm", ".jpg", ".jpeg", ".png"];

    public MediaService(DealContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }
    public async Task<Response<Media>> DeleteAsync(int id, string itemId)
    {
        try
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel?.Medias != null)
            {
                var mediaToRemove = hotel.Medias.FirstOrDefault(m => m.Id.ToString() == itemId);
                if (mediaToRemove != null)
                {
                    hotel.Medias.Remove(mediaToRemove);
                    _context.Entry(hotel).Property(h => h.Medias).IsModified = true;
                    await _context.SaveChangesAsync();
                    return new Response<Media>(mediaToRemove);
                }
                return new Response<Media>("Media not found");
            }
            return new Response<Media>("Hotel not found");
        }
        catch (Exception e)
        {
            return new Response<Media>(e.Message);
        }
    }

    public async Task<Response<Media>> UpdateAsync(int id, MediaDto mediaDto)
    {
        try
        {
            if (mediaDto.MediaFile != null)
            {
                var hotel = await _context.Hotels.FindAsync(id);
                if (hotel?.Medias != null)
                {
                    var mediaToUpdate = hotel.Medias.FirstOrDefault(m => m.Id.ToString() == mediaDto.Id);
                    if (mediaToUpdate != null)
                    {
                        var mediaRes = await _fileService.SaveFileAsync(mediaDto.MediaFile, _allowedMediaExtentions);
                        if (mediaRes.Success)
                        {
                            mediaToUpdate.Path = mediaRes.Item;
                            _context.Entry(hotel).Property(h => h.Medias).IsModified = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                    return new Response<Media>("Media not found");
                }
                return new Response<Media>("Hotel not found");
            }
            return new Response<Media>("MediaFile not found");
        }
        catch (Exception e)
        {
            return new Response<Media>(e.Message);
        }
    }
}