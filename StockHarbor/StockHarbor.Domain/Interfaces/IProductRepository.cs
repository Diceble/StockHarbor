using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, bool includeRelatedEntities);
    Task<IEnumerable<Product>> GetAllAsync(bool includeRelatedEntities);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(int supplierId, bool includeRelatedEntities);
    Task<Product?> GetProductWithSuppliersAsync(int productId, bool includeRelatedEntities);
}
