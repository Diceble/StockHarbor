using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;

namespace StockHarbor.API.Endpoints.Products;

[HttpPut("/api/product")]
[AllowAnonymous]
public class UpdateProductEndpoint : Endpoint<UpdateProductRequest,UpdateProductResponse,UpdateProductMapper>
{
    private readonly IProductService _productService;

    public UpdateProductEndpoint(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task HandleAsync(UpdateProductRequest request, CancellationToken ct)
    {
        try
        {
            var product = Map.ToEntity(request);
            var result = await _productService.UpdateProductAsync(product);
            await SendMappedAsync(result, 200, ct);
        }
        catch (Exception ex)
        {
            await SendErrorsAsync(500,ct);
        }
    }
}
