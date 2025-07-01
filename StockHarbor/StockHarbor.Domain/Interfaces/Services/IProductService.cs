using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces.Services;
public interface IProductService
{
    Task<Product> CreateProductAsync(Product request);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> UpdateProductAsync(Product product);
}
