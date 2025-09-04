using AutoMapper;
using DealManagementSystem.Domain.DTO;
using DealManagementSystem.Domain.Models;

namespace DealManagementSystem.Domain.Mapping;

public class DtoToModel : Profile
{
    public DtoToModel()
    {
        CreateMap<DealDto, Deal>()
          .ForMember(d => d.Hotels, opt => opt.MapFrom(d => d.Hotels));
        CreateMap<HotelDto, Hotel>();
    }
}

