using StockHarbor.Domain.Enums;
using StockHarbor.Domain.ValueObjects;

namespace StockHarbor.API.Models.Products.Request;

public record UpdateProductRequest(int Id, string Name, string Description, string Sku, ProductStatus Status, ProductType ProductType, Dimension? Dimension);
