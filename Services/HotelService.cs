using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services;
using DealManagementSystem.Domain.Services.Communication;
using DealManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DealManagementSystem.Services;

public class HotelService : IHotelService
{
    private readonly DealContext _context;
    public HotelService(DealContext context)
    {
        _context = context;
    }
    public async Task<Response<Hotel>> DeleteAsync(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            return new Response<Hotel>("Hotel not Found");
        }
        try
        {
            var deal = await _context.Deals
                .Include(d => d.Hotels)
                .FirstOrDefaultAsync(d => d.Id == hotel.DealId);

            if (deal != null)
            {
                if (deal.Hotels.Count > 1)
                {
                    _context.Hotels.Remove(hotel);
                    await _context.SaveChangesAsync();
                    return new Response<Hotel>(hotel);
                }
            }
            else
            {
                return new Response<Hotel>("Deal not Found");
            }
            return new Response<Hotel>("Final Hotel in Deal");
        }
        catch (Exception ex)
        {
            return new Response<Hotel>($"{ex.Message}");
        }
    }
}

