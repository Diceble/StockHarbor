﻿using FastEndpoints;
using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;

namespace StockHarbor.API.Mappers.Product;

public class ProductMapper : Mapper<ProductRequest, ProductResponse, Domain.Entities.Product>
{
    public override ProductResponse FromEntity(Domain.Entities.Product e)
    {
        return new ProductResponse(e.Id, e.Name, e.Description, e.Sku, e.Status, e.ProductType, e.CreatedDate, e.Dimension);
    }

    public override Task<ProductResponse> FromEntityAsync(Domain.Entities.Product e, CancellationToken ct)
    {
        return Task.FromResult(new ProductResponse(e.Id, e.Name, e.Description, e.Sku, e.Status, e.ProductType, e.CreatedDate, e.Dimension));
    }
}
