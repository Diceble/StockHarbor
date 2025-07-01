using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces.Repository;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> AddAsync(Product product);
    Task<Product?> UpdateAsync(Product product);
}
