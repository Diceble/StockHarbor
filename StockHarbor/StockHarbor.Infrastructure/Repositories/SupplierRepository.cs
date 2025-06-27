using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Interfaces.Repository;

namespace StockHarbor.Infrastructure.Repositories;
public class SupplierRepository : ISupplierRepository
{
    public Task AddAsync(Supplier supplier)
    {
        throw new NotImplementedException();
    }

    public void Delete(Supplier supplier)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Supplier>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Supplier?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier?> GetSupplierWithProductsAsync(int supplierId)
    {
        throw new NotImplementedException();
    }

    public void Update(Supplier supplier)
    {
        throw new NotImplementedException();
    }
}
