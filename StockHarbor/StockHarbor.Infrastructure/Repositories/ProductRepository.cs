using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain;
using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Interfaces;

namespace StockHarbor.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    public readonly StockHarborDatabaseContext _context;

    public ProductRepository(StockHarborDatabaseContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public Task AddProductVariantAsync(int productId, ProductVariant variant)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int productId)
    {
        var productToDelete = await _context.Products.FindAsync(productId);
        if (productToDelete != null)
        {
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync(bool includeRelatedEntities = false)
    {
        IQueryable<Product> query = _context.Products;
        if (includeRelatedEntities)
        {
            query = query.Include(p => p.Variants);
        }
        return await query.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id, bool includeRelatedEntities = false)
    {
        IQueryable<Product> query = _context.Products;

        if (includeRelatedEntities)
        {
            query = query.Include(p => p.Variants);
        }

        return await query.FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<IEnumerable<Product>> GetProductsByProductVariantIdAsync(int productVariantId, bool includeRelatedEntities = false)
    {
        IQueryable<Product> query = _context.Products;
        if (includeRelatedEntities)
        {
            query = query.Include(p => p.Variants);
        }
        query.Where(p => p.Variants.Any(ps => ps.ProductVariantId == productVariantId));

        return await query.ToListAsync();
    }

    public Task RemoveProductVariantAsync(int variantId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="updatedProduct"></param>
    /// <returns></returns>
    /// <exception cref="Exception">when product is not found exception is thrown</exception>
    public async Task UpdateAsync(Product updatedProduct)
    {
        // 1. Load existing product with related ProductSuppliers
        var existingProduct = await _context.Products
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.ProductId == updatedProduct.ProductId);

        if (existingProduct == null)
            throw new Exception("Product not found");

        // 2. Update scalar properties
        existingProduct.ProductName = updatedProduct.ProductName;
               
        // 4. Save changes
        await _context.SaveChangesAsync();
    }

    public Task UpdateProductVariantAsync(ProductVariant variant)
    {
        throw new NotImplementedException();
    }
}
