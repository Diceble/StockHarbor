using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain;
public class StockHarborDatabaseContext : DbContext
{
    public StockHarborDatabaseContext(DbContextOptions<StockHarborDatabaseContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<ProductVariantSupplier> ProductVariantSuppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
        .HasMany(p => p.Variants)
        .WithOne(v => v.Product)
        .HasForeignKey(v => v.ProductId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductVariant>()
           .OwnsOne(pv => pv.Price, money =>
           {
               money.Property(m => m.Amount).HasColumnName("PriceAmount");
               money.Property(m => m.Currency).HasColumnName("PriceCurrency");
           });

        modelBuilder.Entity<ProductVariantSupplier>()
            .HasKey(pvs => new { pvs.ProductVariantId, pvs.SupplierId });

        modelBuilder.Entity<ProductVariantSupplier>()
            .HasOne(pvs => pvs.ProductVariant)
            .WithMany(pv => pv.ProductVariantSuppliers)
            .HasForeignKey(pvs => pvs.ProductVariantId);

        modelBuilder.Entity<ProductVariantSupplier>()
            .HasOne(pvs => pvs.Supplier)
            .WithMany(s => s.ProductVariantSuppliers)
            .HasForeignKey(pvs => pvs.SupplierId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=StockHarbor;Username=StockHarbor;Password=StockHarborPassword");
        }
    }
}
