using DealManagementSystem.Domain.DTO;

namespace DealManagementSystem.Domain.Models;

public class Deal
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public VideoDto? Video { get; set; }
    public string? Image { get; set; }
    public ICollection<Hotel>Hotels { get; } = new List<Hotel>();
}
