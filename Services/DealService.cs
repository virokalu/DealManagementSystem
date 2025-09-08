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
            .FirstOrDefaultAsync(d => d.Slug == slug);
        if (deal == null)
        {
            return new Response<Deal>("Deal not Found");
        }

        return new Response<Deal>(deal);
    }

    public async Task<Response<Deal>> SaveAsync(Deal deal, IFormFile? imageFile)
    {
        try
        {  
            await _dealValidator.ValidateAndThrowAsync(deal);
            var slugDeal = _context.Deals.FirstOrDefault(d => d.Slug == deal.Slug);
            if (slugDeal != null)
            {
                return new Response<Deal>("Slug Already Exist");
            }
            var createdImageName = await _fileService.SaveFileAsync(imageFile, _allowedFileExtentions);
            if (!createdImageName.Success)
            {
                return new Response<Deal>(createdImageName.Message);
            }
            deal.Image = createdImageName.Item;

            await _context.Deals.AddAsync(deal);
            foreach (var hotel in deal.Hotels)
            {
                hotel.Deal = deal;
            }
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

    public async Task<Response<Deal>> UpdateAsync(int id, Deal deal, IFormFile? imageFile)
    {
        try
        {
            await _dealValidator.ValidateAndThrowAsync(deal);
            var existingDeal = await _context.Deals
                .Include(d => d.Hotels)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (existingDeal == null)
            {
                return new Response<Deal>("Deal not found.");
            }
            if (imageFile != null)
            {
                var createdImageName = await _fileService.SaveFileAsync(imageFile, _allowedFileExtentions);
                if (!createdImageName.Success)
                {
                    return new Response<Deal>(createdImageName.Message);
                }
                deal.Image = createdImageName.Item;
            }

            _context.Entry(existingDeal).CurrentValues.SetValues(deal);
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

}