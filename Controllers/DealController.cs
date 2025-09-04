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
    public async Task<ActionResult<DealDto>> PostDeal([FromBody] DealDto dealDto)
    {
        var deal = _mapper.Map<DealDto, Deal>(dealDto);
        var response = await _dealService.SaveAsync(deal);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        var dealRes = _mapper.Map<Deal, DealDto>(response.Item!);
        return Ok(dealRes);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DealDto>> PutDeal(int id, [FromBody] DealDto dealDto)
    {
        var deal = _mapper.Map<DealDto, Deal>(dealDto);
        var response = await _dealService.UpdateAsync(id, deal);
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