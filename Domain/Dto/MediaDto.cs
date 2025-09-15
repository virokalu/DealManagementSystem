namespace DealManagementSystem.Domain.Models;

public class MediaDto
{
    public IFormFile? MediaFile { get; set; }
    public string? Path { get; set; }
    public string? Alt { get; set; } 
}