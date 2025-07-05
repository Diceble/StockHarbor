using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;

namespace StockHarbor.API.Mappers.Product;

public class CreateProductMapper : Mapper<CreateProductRequest, CreateProductResponse, Domain.Entities.Product>
{
    public override Domain.Entities.Product ToEntity(CreateProductRequest r)
    {   
        return new()
        {
            Name = r.Name ?? string.Empty,
            Description = r.Description,
            Sku = r.Sku ?? string.Empty,
            Status = r.Status,
            ProductType = r.ProductType,
        };
    }

    public override CreateProductResponse FromEntity(Domain.Entities.Product e)
    {
        return new CreateProductResponse(e.Id, e.Name, e.Description, e.Sku, e.Status, e.ProductType);
    }

    public override Task<CreateProductResponse> FromEntityAsync(Domain.Entities.Product e, CancellationToken ct)
    {
        return Task.FromResult(new CreateProductResponse(e.Id, e.Name, e.Description, e.Sku, e.Status, e.ProductType));
    }
}
