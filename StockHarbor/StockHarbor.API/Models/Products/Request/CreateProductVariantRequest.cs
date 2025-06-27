using StockHarbor.Domain.Enums;

namespace StockHarbor.API.Models.Products.Request;

public record CreateProductVariantRequest(string Name, string Description, decimal Price, string Currency, string SKU, ProductVariantStatus Status);