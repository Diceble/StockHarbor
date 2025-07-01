using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain.Entities;

namespace StockHarbor.Infrastructure;
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
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=StockHarbor;Username=StockHarbor;Password=StockHarborPassword");
        }
    }
}
