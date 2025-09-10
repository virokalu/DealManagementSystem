namespace DealManagementSystem.Domain.DTO;
public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; }
    public string Amenities { get; set; }
    public List<string>? Media { get; set; } = new List<string>();
    public List<IFormFile>? MediaFiles { get; set; }
}