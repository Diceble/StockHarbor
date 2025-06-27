using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;

namespace StockHarbor.API.Endpoints.Products;

[HttpPost("/api/product/create")]
[AllowAnonymous]
public class CreateProductEndpoint : Endpoint<CreateProductRequest, CreateProductResponse, CreateProductMapper>
{
    private readonly IProductService _productService;

    public CreateProductEndpoint(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task HandleAsync(CreateProductRequest request, CancellationToken ct)
    {
        try
        {
            var entity = Map.ToEntity(request);
            var result = await _productService.CreateProductAsync(entity);
            await SendMappedAsync(result, 200, ct);
        }
        catch (Exception ex)
        {
            await SendErrorsAsync(default, ct);
        }
    }
}
