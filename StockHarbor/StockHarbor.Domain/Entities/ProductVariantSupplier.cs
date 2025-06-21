namespace StockHarbor.Domain.Entities;
public class ProductVariantSupplier
{
    public int ProductVariantSupplierId { get; set; }
    public int ProductVariantId {  get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public string SupplierProductCode { get; set; } = string.Empty;

}
