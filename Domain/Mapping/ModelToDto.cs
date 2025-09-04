using AutoMapper;
using DealManagementSystem.Domain.DTO;
using DealManagementSystem.Domain.Models;

namespace DealManagementSystem.Domain.Mapping;

public class ModelToDto : Profile
{
    public ModelToDto()
    {
        CreateMap<Deal, DealListDto>();
        CreateMap<Deal, DealDto>()
          .ForMember(d => d.Hotels, opt => opt.MapFrom(d => d.Hotels));
        CreateMap<Hotel, HotelDto>();
    }
}

