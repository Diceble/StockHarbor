using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces.Repository;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, bool includeRelatedEntities);
    Task<IEnumerable<Product>> GetAllAsync(bool includeRelatedEntities);
    Task AddAsync(Product product);
    Task<Product?> UpdateAsync(Product product);
    Task AddProductVariantAsync(int productId, ProductVariant variant);
    Task<ProductVariant?> UpdateProductVariantAsync(ProductVariant variant);
    Task RemoveProductVariantAsync(int variantId);
    Task<Product?> AddProductWithVariants(Product product);
}
