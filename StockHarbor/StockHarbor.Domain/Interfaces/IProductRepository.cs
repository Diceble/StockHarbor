using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, bool includeRelatedEntities);
    Task<IEnumerable<Product>> GetAllAsync(bool includeRelatedEntities);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task<IEnumerable<Product>> GetProductsByProductVariantIdAsync(int supplierId, bool includeRelatedEntities);
    Task AddProductVariantAsync(int productId, ProductVariant variant);
    Task UpdateProductVariantAsync(ProductVariant variant);
    Task RemoveProductVariantAsync(int variantId);
}
