using FastEndpoints;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;
using System.Net;

namespace StockHarbor.API.Endpoints.Products;

[HttpPut("/api/product")]
public class UpdateProductEndpoint(IProductService productService) : Endpoint<UpdateProductRequest, UpdateProductResponse, UpdateProductMapper>
{
    public override async Task HandleAsync(UpdateProductRequest request, CancellationToken ct)
    {
        var product = Map.ToEntity(request);
        var result = await productService.UpdateProductAsync(product);
        await SendMappedAsync(result, (int)HttpStatusCode.OK, ct);
    }
}