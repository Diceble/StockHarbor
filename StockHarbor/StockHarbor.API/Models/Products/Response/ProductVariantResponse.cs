using StockHarbor.Domain.Enums;
using StockHarbor.Domain.ValueObjects;

namespace StockHarbor.API.Models.Products.Response;

public record ProductVariantResponse(int ProductVariantId, string Name, string Description, Money Price, string Sku, ProductStatus Status, int ProductId);
