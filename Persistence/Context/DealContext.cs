using DealManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace DealManagementSystem.Persistence.Context;

public class DealContext: DbContext
{
    public DealContext(DbContextOptions<DealContext> options) : base(options) { }
    public DbSet<Deal> Deals { get; set; } = null!;
    public DbSet<Hotel> Hotels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Deal>().Property(d => d.Name).IsRequired();
      modelBuilder.Entity<Deal>().Property(d => d.Slug).IsRequired();
      modelBuilder.Entity<Hotel>().Property(h => h.Name).IsRequired();
      modelBuilder.Entity<Deal>()
        .HasMany(d => d.Hotels)
        .WithOne(d => d.Deal)
        .HasForeignKey(d => d.DealId)
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<Deal>().OwnsOne(d => d.Video).ToJson();
    }
}