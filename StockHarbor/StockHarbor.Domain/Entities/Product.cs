using StockHarbor.Domain.Enums;

namespace StockHarbor.Domain.Entities;
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Sku { get; set; } 
    public ProductStatus Status { get; set; } // Optional: Active/Inactive, etc.
}
