using MetallurgyApp.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace MetallurgyApp.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ProductionArea> ProductionAreas => Set<ProductionArea>();
    public DbSet<Equipment> Equipments => Set<Equipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductionArea>()
            .HasMany(a => a.Equipments)
            .WithOne(e => e.ProductionArea)
            .HasForeignKey(e => e.ProductionAreaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}