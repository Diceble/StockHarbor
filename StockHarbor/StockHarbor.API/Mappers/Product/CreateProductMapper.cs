using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Entities;

namespace StockHarbor.API.Mappers.Product;

public class CreateProductMapper : Mapper<CreateProductRequest, CreateProductResponse, Domain.Entities.Product>
{
    public override Domain.Entities.Product ToEntity(CreateProductRequest r)
    {
        var mappedVariants = r.ProductVariants?
             .Select(v => new ProductVariant() { Name = v.Name, Description = v.Description, Price = new Money(v.Price, v.Currency), SKU = v.SKU, Status = v.Status })
             .ToList() ?? new();

        return new()
        {
            ProductName = r.ProductName,
            Variants = mappedVariants
        };
    }

    public override CreateProductResponse FromEntity(Domain.Entities.Product e)
    {
        var mappedVariants = e.Variants
            .Select(v => new CreateProductVariantResponse(v.ProductVariantId, v.Name, v.Description, v.Price,v.SKU,v.Status,v.ProductId))
            .ToList() ?? new();
        return new CreateProductResponse(e.ProductId, e.ProductName, mappedVariants);
    }

    public override Task<CreateProductResponse> FromEntityAsync(Domain.Entities.Product e, CancellationToken ct)
    {
        var mappedVariants = e.Variants
            .Select(v => new CreateProductVariantResponse(v.ProductVariantId, v.Name, v.Description, v.Price, v.SKU, v.Status, v.ProductId))
            .ToList() ?? new();

        return Task.FromResult(new CreateProductResponse(e.ProductId, e.ProductName, mappedVariants));
    }
}
