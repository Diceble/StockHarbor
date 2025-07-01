namespace StockHarbor.API.Models.Products.Request;

public record UpdateProductRequest(int ProductId, string ProductName, IEnumerable<UpdateProductVariantRequest> ProductVariants);
