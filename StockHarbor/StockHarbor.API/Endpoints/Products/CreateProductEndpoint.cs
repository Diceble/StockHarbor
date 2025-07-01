using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;

namespace StockHarbor.API.Endpoints.Products;

[HttpPost("/api/product/create")]
[AllowAnonymous]
public class CreateProductEndpoint(IProductService productService) : Endpoint<CreateProductRequest, CreateProductResponse, CreateProductMapper>
{
    public override async Task HandleAsync(CreateProductRequest request, CancellationToken ct)
    {
        var productEntity = Map.ToEntity(request);
        var result = await productService.CreateProductAsync(productEntity);
        await SendMappedAsync(result, 200, ct);
    }
}
