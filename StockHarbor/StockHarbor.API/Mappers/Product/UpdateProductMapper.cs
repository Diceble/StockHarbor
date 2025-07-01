using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Entities;

namespace StockHarbor.API.Mappers.Product;

public class UpdateProductMapper : Mapper<UpdateProductRequest, UpdateProductResponse, Domain.Entities.Product>
{
    public override Domain.Entities.Product ToEntity(UpdateProductRequest r)
    {
        
        return new()
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            Sku = r.Sku,
            Status = r.Status
        };
    }

    public override UpdateProductResponse FromEntity(Domain.Entities.Product e)
    {
        return new UpdateProductResponse(e.Id, e.Name, e.Description, e.Sku, e.Status);
    }

    public override Task<UpdateProductResponse> FromEntityAsync(Domain.Entities.Product e, CancellationToken ct)
    {
        return Task.FromResult(new UpdateProductResponse(e.Id, e.Name, e.Description, e.Sku, e.Status));
    }
}