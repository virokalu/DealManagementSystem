namespace DealManagementSystem.Domain.Models;

public class Media
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Path { get; set; }
    public string? Alt { get; set; }
}
