using DealManagementSystem.Domain.DTO;
using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services;
using DealManagementSystem.Domain.Services.Communication;
using DealManagementSystem.Persistence.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DealManagementSystem.Services;

public class DealService : IDealService
{
    private readonly DealContext _context;
    private readonly IValidator<Deal> _dealValidator;
    private readonly string[] _allowedFileExtentions = [".jpg", ".jpeg", ".png"];
    private readonly string[] _allowedVideoExtentions = [".mp4", ".avi", ".mov", ".webm"];
    private readonly IFileService _fileService;
    public DealService(DealContext context, IValidator<Deal> dealValidator, IFileService fileService)
    {
        _context = context;
        _dealValidator = dealValidator;
        _fileService = fileService;
    }
    public async Task<IEnumerable<Deal>> ListAsync()
    {
        return await _context.Deals.ToListAsync();
    }
    public async Task<Response<Deal>> FindByIdAsync(int id)
    {
        var deal = await _context.Deals.FindAsync(id);
        if (deal == null)
        {
            return new Response<Deal>("Deal not Found");
        }

        return new Response<Deal>(deal);
    }

    public async Task<Response<Deal>> FindBySlugAsync(string slug)
    {
        var deal = await _context.Deals
            .Include(d => d.Hotels)
            .Include(d => d.Video)
            .FirstOrDefaultAsync(d => d.Slug == slug);
        if (deal == null)
        {
            return new Response<Deal>("Deal not Found");
        }

        return new Response<Deal>(deal);
    }

    public async Task<Response<Deal>> SaveAsync(DealDto dealDto)
    {
        try
        {
            var deal = new Deal
            {
                Id = 0,
                Slug = dealDto.Slug,
                Name = dealDto.Name,
                // Video = dealDto.Video,
            };
            if (dealDto.Video != null)
            {
                deal.Video = new Video
                {
                    Id = 0,
                    Path = null,
                    Alt = dealDto.Video.Alt,
                };
            }
            if (dealDto.Hotels != null)
                foreach (HotelDto hotel in dealDto.Hotels)
                {
                    deal.Hotels.Add(new Hotel
                    {
                        Id = 0,
                        Name = hotel.Name,
                        Rate = hotel.Rate,
                        Amenities = hotel.Amenities,
                    });
                }
            await _dealValidator.ValidateAndThrowAsync(deal);
            var slugDeal = _context.Deals.FirstOrDefault(d => d.Slug == deal.Slug);
            if (slugDeal != null)
            {
                return new Response<Deal>("Slug Already Exist");
            }

            //Save Image
            var createdImageName = await _fileService.SaveFileAsync(dealDto.ImageFile, _allowedFileExtentions);
            if (!createdImageName.Success)
            {
                return new Response<Deal>(createdImageName.Message);
            }

            //Save Video
            var createdVideoName = await _fileService.SaveFileAsync(dealDto.VideoFile, _allowedVideoExtentions);
            if (!createdVideoName.Success)
            {
                return new Response<Deal>(createdVideoName.Message);
            }

            //Assign Locations
            deal.Image = createdImageName.Item;
            if (deal.Video != null) deal.Video.Path = createdVideoName.Item;

            //Assign Deal
            await _context.Deals.AddAsync(deal);
            foreach (var hotel in deal.Hotels)
            {
                hotel.Deal = deal;
            }
            if (deal.Video != null) deal.Video.Deal = deal;

            await _context.SaveChangesAsync();
            return new Response<Deal>(deal);
        }
        catch (ValidationException ex)
        {
            var message = ex.Errors.Select(e => e.ErrorMessage).ToList();
            return new Response<Deal>($"{string.Join(",", message)}");
        }
        catch (Exception ex)
        {
            return new Response<Deal>($"{ex.Message}");
        }
    }

    public async Task<Response<Deal>> UpdateAsync(int id, Deal deal)
    {
        try
        {
            await _dealValidator.ValidateAndThrowAsync(deal);
            var existingDeal = await _context.Deals
                .Include(d => d.Hotels)
                .Include(d => d.Video)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (existingDeal == null)
            {
                return new Response<Deal>("Deal not found.");
            }
            // if (imageFile != null)
            // {
            //     var createdImageName = await _fileService.SaveFileAsync(imageFile, _allowedFileExtentions);
            //     if (!createdImageName.Success)
            //     {
            //         return new Response<Deal>(createdImageName.Message);
            //     }
            //     deal.Image = createdImageName.Item;
            // }

            // _context.Entry(existingDeal).CurrentValues.SetValues(deal);

            existingDeal.Name = deal.Name;
            if (existingDeal.Video != null)
            {
                existingDeal.Video.Alt = deal.Video.Alt;
            }
            else
            {
                existingDeal.Video = new Video
                {
                    Id = 0,
                    Path = null,
                    Alt = deal.Video.Alt,
                    DealId = existingDeal.Id,
                    Deal = existingDeal
                };
            }

            foreach (var hotel in deal.Hotels)
            {
                var hotelEntity = existingDeal.Hotels.FirstOrDefault(c => c.Id == hotel.Id && c.Id != 0);
                if (hotelEntity != null)
                {
                    hotelEntity.Name = hotel.Name;
                    hotelEntity.Rate = hotel.Rate;
                    hotelEntity.Amenities = hotel.Amenities;
                }
                else
                {
                    hotel.Deal = existingDeal;
                    hotel.DealId = existingDeal.Id;
                    existingDeal.Hotels.Add(hotel);
                }
            }
            await _context.SaveChangesAsync();
            return new Response<Deal>(existingDeal);
        }
        catch (ValidationException ex)
        {
            var message = ex.Errors.Select(e => e.ErrorMessage).ToList();
            return new Response<Deal>($"{string.Join(",", message)}");
        }
        catch (Exception ex)
        {
            return new Response<Deal>($"{ex.Message}");
        }
    }
    public async Task<Response<Deal>> DeleteAsync(int id)
    {
        var deal = await _context.Deals.FindAsync(id);
        if (deal == null)
        {
            return new Response<Deal>("Deal not found");
        }

        try
        {
            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
            return new Response<Deal>(deal);
        }
        catch (Exception ex)
        {
            return new Response<Deal>($"{ex.Message}");
        }
    }

    public async Task<Response<Deal>> ImageUpdate(int id, IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            var existingDeal = await _context.Deals.FindAsync(id);
            if (existingDeal == null)
            {
                return new Response<Deal>("Deal not found.");
            }

            var createdImageName = await _fileService.SaveFileAsync(imageFile, _allowedFileExtentions);
            if (!createdImageName.Success)
            {
                return new Response<Deal>(createdImageName.Message);
            }

            existingDeal.Image = createdImageName.Item;
            await _context.SaveChangesAsync();
            return new Response<Deal>(existingDeal);
        }
        return new Response<Deal>("Image not foumd.");
    }

    public async Task<Response<Deal>> VideoUpdate(int id, IFormFile? videoFile)
    {
        if (videoFile != null)
        {
            var existingDeal = await _context.Deals
                .Include(d => d.Video)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (existingDeal == null)
            {
                return new Response<Deal>("Deal not found.");
            }

            var createdVideoName = await _fileService.SaveFileAsync(videoFile, _allowedVideoExtentions);
            if (!createdVideoName.Success)
            {
                return new Response<Deal>(createdVideoName.Message);
            }

            if (existingDeal.Video != null)
            {
                existingDeal.Video.Path = createdVideoName.Item;
            }
            else
            {
                existingDeal.Video = new Video
                {
                    Id = 0,
                    Path = createdVideoName.Item,
                    Alt = "This is a Alt",
                    DealId = existingDeal.Id,
                    Deal = existingDeal
                };
            }

            await _context.SaveChangesAsync();
            return new Response<Deal>(existingDeal);
        }
        return new Response<Deal>("Video not foumd.");
    }
}