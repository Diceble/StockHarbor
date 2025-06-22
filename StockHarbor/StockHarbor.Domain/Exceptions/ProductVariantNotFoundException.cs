using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHarbor.Domain.Exceptions;
public class ProductVariantNotFoundException : StockHarborException
{
    public ProductVariantNotFoundException(string message) : base(message)
    {
    }

    public ProductVariantNotFoundException(int variantId) : base($"product variant with Id {variantId} is not found") { }
}
