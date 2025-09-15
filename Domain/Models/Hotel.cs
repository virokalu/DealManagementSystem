namespace DealManagementSystem.Domain.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; }
    public string Amenities { get; set; }
    public List<Media>? Medias { set; get; } = new List<Media>();
    public int DealId { get; set; }
    public Deal Deal { get; set; }
}