using FastEndpoints;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;

namespace StockHarbor.API.Endpoints.Products;

[HttpGet("/api/product")]
public class GetAllProductsEndpoint(IProductService productService) : EndpointWithoutRequest<IEnumerable<ProductResponse>, ProductMapper>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var products = await productService.GetAllProductsAsync();
        var mappedProducts = products.Select(Map.FromEntity).ToList();
        await SendAsync(mappedProducts, 200, ct);
    }
}
