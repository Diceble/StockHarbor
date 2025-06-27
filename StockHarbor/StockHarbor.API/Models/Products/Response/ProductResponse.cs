namespace StockHarbor.API.Models.Products.Response;

public record ProductResponse(int ProductId, string ProductName, IEnumerable<ProductVariantResponse> ProductVariants);

