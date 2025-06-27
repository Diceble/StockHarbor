using StockHarbor.Domain.Enums;

namespace StockHarbor.Domain.Entities;
public class ProductVariant
{
    public int ProductVariantId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Money Price { get; set; }

    public string SKU { get; set; } = string.Empty; // Or any unique identifier for the variant
    public ProductVariantStatus Status { get; set; } // Optional: Active/Inactive, etc.

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public ICollection<ProductVariantSupplier> ProductVariantSuppliers { get; set; } = new List<ProductVariantSupplier>();

    public ProductVariant(int productVariantId, string name, string description, Money price, string sKU, ProductVariantStatus status, int productId)
    {
        ProductVariantId = productVariantId;
        Name = name;
        Description = description;
        Price = price;
        SKU = sKU;
        Status = status;
        ProductId = productId;
    }

    public ProductVariant(string name, string description, Money price, string sKU, ProductVariantStatus status)
    {
        Name = name;
        Description = description;
        Price = price;
        SKU = sKU;
        Status = status;
    }

    public ProductVariant() { }
}