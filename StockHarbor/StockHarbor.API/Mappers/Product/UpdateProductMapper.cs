using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Entities;

namespace StockHarbor.API.Mappers.Product;

public class UpdateProductMapper : Mapper<UpdateProductRequest, UpdateProductResponse, Domain.Entities.Product>
{
    public override Domain.Entities.Product ToEntity(UpdateProductRequest r)
    {
        var mappedVariants = (r.ProductVariants)
             .Select(v => new ProductVariant()
             {
                 ProductVariantId = v.ProductVariantId,
                 Name = v.Name ?? string.Empty,
                 Description = v.Description ?? string.Empty,
                 Price = new Money(v.Price, v.Currency ?? ""),
                 SKU = v.SKU ?? string.Empty,
                 Status = v.Status,
                 ProductId = r.ProductId
             })
             .ToList();

        return new()
        {
            ProductId = r.ProductId,
            ProductName = r.ProductName ?? string.Empty,
            Variants = mappedVariants
        };
    }

    public override UpdateProductResponse FromEntity(Domain.Entities.Product e)
    {
        var mappedVariants = (e.Variants)
            .Select(v => new UpdateProductVariantResponse(v.ProductVariantId, v.Name, v.Description, v.Price, v.SKU, v.Status, v.ProductId))
            .ToList();
        return new UpdateProductResponse(e.ProductId, e.ProductName, mappedVariants);
    }

    public override Task<UpdateProductResponse> FromEntityAsync(Domain.Entities.Product e, CancellationToken ct)
    {
        var mappedVariants = (e.Variants)
            .Select(v => new UpdateProductVariantResponse(v.ProductVariantId, v.Name, v.Description, v.Price, v.SKU, v.Status, v.ProductId))
            .ToList();

        return Task.FromResult(new UpdateProductResponse(e.ProductId, e.ProductName, mappedVariants));
    }
}