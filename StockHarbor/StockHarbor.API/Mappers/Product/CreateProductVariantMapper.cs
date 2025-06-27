using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Entities;

namespace StockHarbor.API.Mappers.Product;

public class CreateProductVariantMapper : Mapper<CreateProductVariantRequest,CreateProductVariantResponse, ProductVariant>
{
    public override CreateProductVariantResponse FromEntity(ProductVariant e) => new(e.ProductVariantId, e.Name, e.Description, e.Price, e.SKU, e.Status, e.ProductId);

    public override ProductVariant ToEntity(CreateProductVariantRequest r) => new() { Name = r.Name, Description = r.Description, Price = new Money(r.Price, r.Currency), SKU = r.SKU, Status = r.Status};
}
