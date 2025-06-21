using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHarbor.Domain.Entities;
public class ProductVariant
{
    public int ProductVariantId { get; set; }
    public string SKU { get; set; } = string.Empty; // Or any unique identifier for the variant
    public VariantStatus Status { get; set; } // Optional: Active/Inactive, etc.

    public int ProductId { get; set; }
    public required Product Product { get; set; }

    public ICollection<ProductVariantSupplier> ProductVariantSuppliers { get; set; } = new List<ProductVariantSupplier>();
}

public enum VariantStatus
{
    Active = 0,
    Inactive = 1,
    Archived = 2
}