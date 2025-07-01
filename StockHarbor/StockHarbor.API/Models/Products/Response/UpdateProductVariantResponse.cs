using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Enums;

namespace StockHarbor.API.Models.Products.Response;

public record UpdateProductVariantResponse(int ProductVariantId, string Name, string Description, Money Price, string Sku, ProductVariantStatus Status, int ProductId) 
    : ProductVariantResponse(ProductVariantId, Name, Description, Price, Sku, Status, ProductId);

