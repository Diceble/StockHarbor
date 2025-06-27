using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;

namespace StockHarbor.API.Mappers.Product;

public class CreateProductMapper : Mapper<CreateProductRequest,CreateProductResponse, Domain.Entities.Product>
{
    public override Domain.Entities.Product ToEntity(CreateProductRequest r)
    {
        return base.ToEntity(r);
    }

    public override CreateProductResponse FromEntity(Domain.Entities.Product e)
    {
        return base.FromEntity(e);
    }
}
