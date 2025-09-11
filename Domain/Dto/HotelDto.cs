using DealManagementSystem.Domain.Models;

namespace DealManagementSystem.Domain.DTO;
public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; }
    public string Amenities { get; set; }
    public List<MediaDto>? Media { get; set; } = new List<MediaDto>();
}