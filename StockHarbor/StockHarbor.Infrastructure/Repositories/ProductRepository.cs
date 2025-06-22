using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain;
using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Enums;
using StockHarbor.Domain.Exceptions;
using StockHarbor.Domain.Interfaces;

namespace StockHarbor.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    public readonly StockHarborDatabaseContext _context;

    public ProductRepository(StockHarborDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Add product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Add a product variant only if product is found
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="variant"></param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException">when variant has no existing product</exception>
    public async Task AddProductVariantAsync(int productId, ProductVariant variant)
    {
        var product = await _context.Products
        .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
        {
            throw new ProductNotFoundException(productId);
        }

        await _context.ProductVariants.AddAsync(variant);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes product if found
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException">if no product is found throws this exception</exception>
    public async Task DeleteAsync(int productId)
    {
        var productToDelete = await _context.Products.FindAsync(productId);
        if (productToDelete != null)
        {
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ProductNotFoundException(productId);
        }
    }

    /// <summary>
    /// Gets all products
    /// </summary>
    /// <param name="includeRelatedEntities">when true fetches all foreign key relations for product </param>
    /// <returns></returns>
    public async Task<IEnumerable<Product>> GetAllAsync(bool includeRelatedEntities = false)
    {
        IQueryable<Product> query = _context.Products;
        if (includeRelatedEntities)
        {
            query = query.Include(p => p.Variants);
        }
        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets one product based on it's id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeRelatedEntities">when true fetches all foreign key relations for product </param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException">when product is not found for given id</exception>
    public async Task<Product?> GetByIdAsync(int id, bool includeRelatedEntities = false)
    {
        IQueryable<Product> query = _context.Products;

        if (includeRelatedEntities)
        {
            query = query.Include(p => p.Variants);
        }

        return await query.FirstOrDefaultAsync(p => p.ProductId == id) ?? throw new ProductNotFoundException(id);
    }

    /// <summary>
    /// Tries to soft delete Product variant by switching status to deleted
    /// </summary>
    /// <param name="variantId">id of the variant you are trying to delete</param>
    /// <returns></returns>
    /// <exception cref="ProductVariantNotFoundException">when product variant is not found</exception>
    public async Task RemoveProductVariantAsync(int variantId)   
    {
        var variant = await _context.ProductVariants
            .FirstOrDefaultAsync(v => v.ProductVariantId == variantId);

        if (variant == null) throw new ProductVariantNotFoundException(variantId);

        variant.Status = ProductVariantStatus.Deleted;
        await _context.SaveChangesAsync();
    }
    

    /// <summary>
    /// updating the product entity
    /// </summary>
    /// <param name="updatedProduct"></param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException"> when product trying to update is not found</exception>
    public async Task UpdateAsync(Product updatedProduct)
    {
        // 1. Load existing product with related ProductSuppliers
        var existingProduct = await _context.Products
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.ProductId == updatedProduct.ProductId);

        if (existingProduct == null)
            throw new ProductNotFoundException(updatedProduct.ProductId);

        // 2. Update scalar properties
        existingProduct.ProductName = updatedProduct.ProductName;
               
        // 4. Save changes
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update product variant
    /// </summary>
    /// <param name="variant"></param>
    /// <returns></returns>
    /// <exception cref="ProductVariantNotFoundException">when product variant trying to update is not found </exception>
    public async Task UpdateProductVariantAsync(ProductVariant variant)
    {
        var existingVariant = await _context.ProductVariants
        .FirstOrDefaultAsync(v => v.ProductVariantId == variant.ProductVariantId);

        if (existingVariant == null) throw new ProductVariantNotFoundException(variant.ProductVariantId);

        // Update properties manually or use AutoMapper
        existingVariant.Name = variant.Name;
        existingVariant.Price = variant.Price;
        // etc.

        await _context.SaveChangesAsync();
    }
}
