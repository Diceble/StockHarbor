using StockHarbor.Domain.Enums;
using StockHarbor.Domain.ValueObjects;

namespace StockHarbor.API.Models.Products.Response;

public record ProductResponse(int Id, string Name, string Description, string Sku, ProductStatus Status, ProductType ProductType, DateTimeOffset CreatedAt, Dimension? Dimension);

