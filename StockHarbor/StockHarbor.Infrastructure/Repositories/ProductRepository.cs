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
            query = query.Include(p => p.ProductSuppliers);
        }
        return await query.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id, bool includeRelatedEntities = false)
    {
        IQueryable<Product> query = _context.Products;

        if (includeRelatedEntities)
        {
            query = query.Include(p => p.ProductSuppliers);
        }

        return await query.FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(int supplierId, bool includeRelatedEntities = false)
    {
        IQueryable<Product> query = _context.Products;
        if (includeRelatedEntities)
        {
            query = query.Include(p => p.ProductSuppliers);
        }
        query.Where(p => p.ProductSuppliers.Any(ps => ps.SupplierId == supplierId));

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductWithSuppliersAsync(int productId, bool includeRelatedEntities = false)
    {
        return await _context.Products
          .Include(p => p.ProductSuppliers)
              .ThenInclude(ps => ps.Supplier)
          .FirstOrDefaultAsync(p => p.ProductId == productId);
    }

    public async Task UpdateAsync(Product updatedProduct)
    {
        // 1. Load existing product with related ProductSuppliers
        var existingProduct = await _context.Products
            .Include(p => p.ProductSuppliers)
            .FirstOrDefaultAsync(p => p.ProductId == updatedProduct.ProductId);

        if (existingProduct == null)
            throw new Exception("Product not found");

        // 2. Update scalar properties
        existingProduct.ProductName = updatedProduct.ProductName;

        // 3. Handle ProductSuppliers updates

        // Remove ProductSuppliers that no longer exist
        var removedSuppliers = existingProduct.ProductSuppliers
            .Where(ps => !updatedProduct.ProductSuppliers.Any(up => up.ProductSupplierId == ps.ProductSupplierId))
            .ToList();

        foreach (var removed in removedSuppliers)
        {
            existingProduct.ProductSuppliers.Remove(removed);
        }

        // Add or update ProductSuppliers from updatedProduct
        foreach (var updatedSupplier in updatedProduct.ProductSuppliers)
        {
            var existingSupplier = existingProduct.ProductSuppliers
                .FirstOrDefault(ps => ps.ProductSupplierId == updatedSupplier.ProductSupplierId);

            if (existingSupplier == null)
            {
                // New supplier link
                existingProduct.ProductSuppliers.Add(updatedSupplier);
            }
            else
            {
                // Update existing link properties if any
                existingSupplier.SupplierProductCode = updatedSupplier.SupplierProductCode;
            }
        }

        // 4. Save changes
        await _context.SaveChangesAsync();
    }
}
