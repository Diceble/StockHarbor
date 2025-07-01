using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;

namespace StockHarbor.API.Endpoints.Products;

[HttpGet("/api/product")]
[AllowAnonymous]
public class GetAllProductsEndpoint : Endpoint<GetAllProductRequest, IEnumerable<ProductResponse>, ProductMapper>
{
    private readonly IProductService _productService;

    public GetAllProductsEndpoint(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task HandleAsync(GetAllProductRequest request, CancellationToken ct)
    {
        try
        {
            var products = await _productService.GetAllProductsAsync(request.IncludeProductVariants);
            var mappedProducts = products.Select(p => Map.FromEntity(p)).ToList();
            await SendAsync(mappedProducts, 200, ct);
        }
        catch (Exception ex)
        {
            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
        }
    }
}
