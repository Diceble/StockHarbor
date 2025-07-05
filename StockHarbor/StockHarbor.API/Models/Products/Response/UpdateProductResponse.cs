using StockHarbor.Domain.Enums;

namespace StockHarbor.API.Models.Products.Response;

public record UpdateProductResponse(int Id, string Name, string Description, string Sku, ProductStatus Status, ProductType ProductType);
