using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain.Entities;

namespace StockHarbor.Infrastructure.Persistance;
public class StockHarborDatabaseContext : DbContext
{
    public StockHarborDatabaseContext(DbContextOptions<StockHarborDatabaseContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                .UseIdentityAlwaysColumn();
            entity.OwnsOne(p => p.Dimension, dim =>
            {
                dim.Property(d => d.Height).HasColumnName("Dimension_Height");
                dim.Property(d => d.Width).HasColumnName("Dimension_Width");
                dim.Property(d => d.Length).HasColumnName("Dimension_Length");
                dim.Property(d => d.Unit).HasColumnName("Dimension_Unit");
            });
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=StockHarbor.ApiDb;Username=StockHarbor;Password=StockHarborPassword");
        }
    }
}
