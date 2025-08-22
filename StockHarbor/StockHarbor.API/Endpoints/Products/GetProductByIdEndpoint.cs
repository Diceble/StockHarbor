using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.API.Mappers.Product;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Interfaces.Services;
using System.Net;

namespace StockHarbor.API.Endpoints.Products;

[HttpGet("/api/product/{id}")]
[Authorize (Policy = "product.read")]
public class GetProductByIdEndpoint(IProductService productService) : Endpoint<GetProductByIdRequest, ProductResponse, ProductMapper>
{

    public override async Task HandleAsync(GetProductByIdRequest request, CancellationToken ct)
    {
        var product = await productService.GetProductById(request.Id);
        await SendAsync(await Map.FromEntityAsync(product,ct), (int)HttpStatusCode.OK, ct);
    }
}