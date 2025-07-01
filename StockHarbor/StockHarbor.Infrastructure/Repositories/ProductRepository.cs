using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Exceptions;
using StockHarbor.Domain.Interfaces.Repository;

namespace StockHarbor.Infrastructure.Repositories;
public class ProductRepository(StockHarborDatabaseContext context) : IProductRepository
{
    public readonly StockHarborDatabaseContext _dbContext = context;

    /// <summary>
    /// Add product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Product> AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Gets all products
    /// </summary>
    /// <param name="includeProductVariants">when true fetches the variants of that product </param>
    /// <returns></returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        IQueryable<Product> query = _dbContext.Products;

        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets one product based on it's id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeProductVariants">when true fetches the variants of that product </param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">when product is not found for given id</exception>
    public async Task<Product?> GetByIdAsync(int id)
    {
        IQueryable<Product> query = _dbContext.Products;

        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// updating the product entity
    /// </summary>
    /// <param name="updatedProduct"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"> when product trying to update is not found</exception>
    public async Task<Product?> UpdateAsync(Product updatedProduct)
    {
        // 1. Load existing product with related ProductSuppliers
        var existingProduct = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

        if (existingProduct == null)
            return null;

        // 2. Update scalar properties
        existingProduct.Name = updatedProduct.Name;
        existingProduct.Description = updatedProduct.Description;
        existingProduct.Sku = updatedProduct.Sku;
        existingProduct.Status = updatedProduct.Status;

        // 4. Save changes
        await _dbContext.SaveChangesAsync();

        // 5. return updated product
        return existingProduct;
    }
}
