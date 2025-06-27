using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Enums;

namespace StockHarbor.API.Models.Products.Response;

public record ProductVariantResponse(int ProductVariantId, string Name, string Description, Money Price, string Sku, ProductVariantStatus Status, int ProductId);
