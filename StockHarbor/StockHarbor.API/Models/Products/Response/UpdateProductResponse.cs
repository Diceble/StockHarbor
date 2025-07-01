namespace StockHarbor.API.Models.Products.Response;

public record UpdateProductResponse(int ProductId, string ProductName, IEnumerable<UpdateProductVariantResponse> ProductVariants);
