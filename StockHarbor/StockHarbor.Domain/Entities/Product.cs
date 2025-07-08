using StockHarbor.Domain.Enums;
using StockHarbor.Domain.ValueObjects;

namespace StockHarbor.Domain.Entities;
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Sku { get; set; } 
    public required ProductStatus Status { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public Dimension? Dimension { get; set; }
    public required ProductType ProductType { get; set; }
}
