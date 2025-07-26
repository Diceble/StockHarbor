using Microsoft.EntityFrameworkCore;
using StockHarbor.TenantApi.Models.Entities;

namespace StockHarbor.TenantApi.Persistence;

public class TenantDbContext : DbContext
{
    public DbSet<Tenant> Tenants => Set<Tenant>();

    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(t => t.TenantId);
            entity.HasIndex(t => t.TenantId).IsUnique();
            entity.HasIndex(t => t.Status);
            entity.Property(t => t.TenantName).IsRequired().HasMaxLength(200);
            entity.Property(t => t.ConnectionString).IsRequired();
            entity.Property(t => t.CreatedDate).IsRequired();
            entity.Property(t => t.Status).IsRequired();
        });
    }

}
