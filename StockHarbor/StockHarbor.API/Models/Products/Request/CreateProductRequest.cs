using StockHarbor.Domain.Enums;

namespace StockHarbor.API.Models.Products.Request;

public record CreateProductRequest(string Name, string Description, string Sku, ProductStatus Status);
