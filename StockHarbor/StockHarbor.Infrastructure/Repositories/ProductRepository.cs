using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain;
using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Enums;
using StockHarbor.Domain.Exceptions;
using StockHarbor.Domain.Interfaces.Repository;

namespace StockHarbor.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    public readonly StockHarborDatabaseContext _dbContext;

    public ProductRepository(StockHarborDatabaseContext context)
    {
        _dbContext = context;
    }

    /// <summary>
    /// Add product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
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
        var product = await _dbContext.Products
        .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
        {
            throw new ProductNotFoundException(productId);
        }

        await _dbContext.ProductVariants.AddAsync(variant);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///  Adds a product with the variants supplied rolls back if something fails
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"> when there are no variants supplied</exception>
    /// <exception cref="ArgumentNullException"> when product is null</exception>

    public async Task<Product?> AddProductWithVariants(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.Variants == null || product.Variants.Count == 0)
        {
            throw new ArgumentException("product must have atleast one  variant", nameof(product));
        }

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            foreach (var variant in product.Variants)
            {
                variant.ProductId = product.ProductId; // Set FK if not handled via navigation
            }

            await _dbContext.ProductVariants.AddRangeAsync(product.Variants);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return product;
    }

    /// <summary>
    /// Deletes product if found
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    /// <exception cref="ProductNotFoundException">if no product is found throws this exception</exception>
    public async Task DeleteAsync(int productId)
    {
        var productToDelete = await _dbContext.Products.FindAsync(productId);
        if (productToDelete != null)
        {
            await _dbContext.SaveChangesAsync();
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
        IQueryable<Product> query = _dbContext.Products;
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
        IQueryable<Product> query = _dbContext.Products;

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
        var variant = await _dbContext.ProductVariants
            .FirstOrDefaultAsync(v => v.ProductVariantId == variantId);

        if (variant == null) throw new ProductVariantNotFoundException(variantId);

        variant.Status = ProductVariantStatus.Deleted;
        await _dbContext.SaveChangesAsync();
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
        var existingProduct = await _dbContext.Products
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.ProductId == updatedProduct.ProductId);

        if (existingProduct == null)
            throw new ProductNotFoundException(updatedProduct.ProductId);

        // 2. Update scalar properties
        existingProduct.ProductName = updatedProduct.ProductName;

        // 4. Save changes
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Update product variant
    /// </summary>
    /// <param name="variant"></param>
    /// <returns></returns>
    /// <exception cref="ProductVariantNotFoundException">when product variant trying to update is not found </exception>
    public async Task UpdateProductVariantAsync(ProductVariant variant)
    {
        var existingVariant = await _dbContext.ProductVariants
        .FirstOrDefaultAsync(v => v.ProductVariantId == variant.ProductVariantId);

        if (existingVariant == null) throw new ProductVariantNotFoundException(variant.ProductVariantId);

        // Update properties manually or use AutoMapper
        existingVariant.Name = variant.Name;
        existingVariant.Price = variant.Price;
        // etc.

        await _dbContext.SaveChangesAsync();
    }
}
