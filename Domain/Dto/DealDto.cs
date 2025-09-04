namespace DealManagementSystem.Domain.DTO;
public class DealDto : DealListDto
{
    public ICollection<HotelDto>?Hotels { get; set; } = new List<HotelDto>();
}