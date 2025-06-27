using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;

namespace StockHarbor.API.Mappers.Product;

public class ProductMapper : Mapper<ProductRequest,ProductResponse,Domain.Entities.Product>
{
    public override ProductResponse FromEntity(Domain.Entities.Product e)
    {
        var productVariants = e.Variants
            .Select(v => new ProductVariantResponse(v.ProductVariantId, v.Name, v.Description, v.Price, v.SKU, v.Status, v.ProductId))
            .ToList();
        return new ProductResponse(e.ProductId, e.ProductName,productVariants);
    }
}
