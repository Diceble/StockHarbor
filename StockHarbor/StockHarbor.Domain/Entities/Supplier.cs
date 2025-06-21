namespace StockHarbor.Domain.Entities;
public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;

    public List<ProductVariantSupplier> ProductVariantSuppliers { get; set; } = [];
}
