using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces.Services;
public interface IProductService
{
    Task<Product?> CreateProductAsync(Product request);
    Task<ProductVariant?> CreateProductVariantAsync(ProductVariant request);
    Task<IEnumerable<Product>> GetAllProducts(bool includeProductVariants);
}
