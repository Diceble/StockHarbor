namespace StockHarbor.API.Models.Products.Request;

public record CreateProductRequest(string ProductName, IEnumerable<CreateProductVariantRequest> ProductVariants);
