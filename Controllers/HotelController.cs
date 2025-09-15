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
    private readonly IMediaService _mediaService;
    public HotelController(IHotelService hotelService, IMapper mapper, IMediaService mediaService)
    {
        _hotelService = hotelService;
        _mapper = mapper;
        _mediaService = mediaService;
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
    [HttpDelete("media/{id}")]
    public async Task<ActionResult<HotelDto>> DeleteMedia(int id, [FromBody] MediaDto media)
    {
        var response = await _mediaService.DeleteAsync(id, media.Id);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        var mediaDto = _mapper.Map<Media, MediaDto>(response.Item!);
        return Ok(mediaDto);
    }
    [HttpPut("media/{id}")]
    public async Task<ActionResult<DealDto>> PutMedia(int id, [FromForm] MediaDto media)
    {        
        var response = await _mediaService.UpdateAsync(id, media);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        var mediaDto = _mapper.Map<Media, MediaDto>(response.Item!);
        return Ok(mediaDto);
    }
}

