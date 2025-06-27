namespace StockHarbor.API.Models.Products.Response;

public record CreateProductResponse(int ProductId, string ProductName, IEnumerable<CreateProductVariantResponse> ProductVariants);