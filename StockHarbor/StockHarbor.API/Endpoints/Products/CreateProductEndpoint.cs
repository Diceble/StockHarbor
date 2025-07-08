using FastEndpoints;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;
using System.Net;

namespace StockHarbor.API.Endpoints.Products;

[HttpPost("/api/product")]
public class CreateProductEndpoint(IProductService productService) : Endpoint<CreateProductRequest, CreateProductResponse, CreateProductMapper>
{
    public override async Task HandleAsync(CreateProductRequest request, CancellationToken ct)
    {
        var productEntity = Map.ToEntity(request);
        var result = await productService.CreateProductAsync(productEntity);
        await SendMappedAsync(result,(int)HttpStatusCode.OK, ct);
    }
}
