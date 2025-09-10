namespace DealManagementSystem.Domain.Models;

public class Video
{
    public int Id { get; set; }
    public string? Path { get; set; }
    public string? Alt { get; set; }
    public int DealId { get; set; }
    public Deal Deal { get; set; }
    
}
