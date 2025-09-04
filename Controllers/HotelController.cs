using AutoMapper;
using DealManagementSystem.Domain.DTO;
using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IMapper _mapper;
    public HotelController(IHotelService hotelService, IMapper mapper)
    {
        _hotelService = hotelService;
        _mapper = mapper;
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<HotelDto>> DeleteHotel(int id)
    {
        var response = await _hotelService.DeleteAsync(id);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        var hotelDto = _mapper.Map<Hotel, HotelDto>(response.Item!);
        return Ok(hotelDto);
    }
}

