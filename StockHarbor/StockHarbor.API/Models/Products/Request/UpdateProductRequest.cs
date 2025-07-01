using StockHarbor.Domain.Enums;

namespace StockHarbor.API.Models.Products.Request;

public record UpdateProductRequest(int Id, string Name, string Description, string Sku, ProductStatus Status);
