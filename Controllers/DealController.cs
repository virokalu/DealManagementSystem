using AutoMapper;
using DealManagementSystem.Domain.DTO;
using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DealController : ControllerBase
{
    private readonly IDealService _dealService;
    private readonly IMapper _mapper;
    public DealController(IDealService dealService, IMapper mapper)
    {
        _mapper = mapper;
        _dealService = dealService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DealListDto>>> GetDeals()
    {
        var deals = await _dealService.ListAsync();
        var dtos = _mapper.Map<IEnumerable<Deal>, IEnumerable<DealListDto>>(deals);
        return Ok(dtos);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<DealDto>> GetDeal(string slug)
    {
        var response = await _dealService.FindBySlugAsync(slug);
        if (!response.Success)
        {
            return NotFound(response.Message);
        }

        var dto = _mapper.Map<Deal, DealDto>(response.Item!);
        return Ok(dto);
    }
    [HttpPost]
    public async Task<ActionResult<DealDto>> PostDeal([FromForm] DealDto dealDto)
    {
        // string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
        // var createdImageName = await _fileService.SaveFileAsync(dealDto.ImageFile, allowedFileExtentions);
        // if (!createdImageName.Success)
        // {
        //     return BadRequest(createdImageName.Message);
        // }
        
        var deal = _mapper.Map<DealDto, Deal>(dealDto);
        // deal.Image = createdImageName.Item;

        var response = await _dealService.SaveAsync(deal, dealDto.ImageFile);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        var dealRes = _mapper.Map<Deal, DealDto>(response.Item!);
        return Ok(dealRes);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DealDto>> PutDeal(int id, [FromForm] DealDto dealDto)
    {
        // if (dealDto.ImageFile != null)
        // {
        //     string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
        //     var createdImageName = await _fileService.SaveFileAsync(dealDto.ImageFile, allowedFileExtentions);
        //     if (!createdImageName.Success)
        //     {
        //         return BadRequest(createdImageName.Message);
        //     }

        //     dealDto.Image = createdImageName.Item;
        // }

        var deal = _mapper.Map<DealDto, Deal>(dealDto);
        var response = await _dealService.UpdateAsync(id, deal, dealDto.ImageFile);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        var dealRes = _mapper.Map<Deal, DealDto>(response.Item!);
        return Ok(dealRes);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DealDto>> DeleteDeal(int id)
    {
        var response = await _dealService.DeleteAsync(id);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        var dealRes = _mapper.Map<Deal, DealDto>(response.Item!);
        return Ok(dealRes);
    }
}