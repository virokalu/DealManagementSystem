namespace DealManagementSystem.Domain.DTO;
public class DealDto : DealListDto
{
    public IFormFile? ImageFile { get; set; }
    public IFormFile? VideoFile { get; set; }
    public VideoDto? Video { get; set; }
    public ICollection<HotelDto>?Hotels { get; set; } = new List<HotelDto>();
}