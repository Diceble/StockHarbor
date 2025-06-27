using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces.Repository;
public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(int id);
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task AddAsync(Supplier supplier);
    void Update(Supplier supplier);
    void Delete(Supplier supplier);
    Task<Supplier?> GetSupplierWithProductsAsync(int supplierId);
}
