using Microsoft.Extensions.Logging;
using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Exceptions;
using StockHarbor.Domain.Interfaces.Repository;
using StockHarbor.Domain.Interfaces.Services;

namespace StockHarbor.Infrastructure.Services;
public class ProductService(IProductRepository productRepository, ILogger<ProductService> logger) : IProductService
{
    public async Task<Product> CreateProductAsync(Product product)
    {
        if (product == null)
        {
            logger.LogWarning("Attempt to create product while product is null");
            throw new ArgumentNullException(nameof(product), "Attempt to create product while product is null");
        }

        logger.LogInformation("Creating product {ProductName} with SKU {Sku}", product.Name, product.Sku);
        product.CreatedDate = DateTimeOffset.UtcNow;
        var result = await productRepository.AddAsync(product);
        return result;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await productRepository.GetAllAsync();
    }

    public async Task<Product> GetProductById(int id)
    {
        if(id < 0)
        {
            logger.LogWarning("Attempting to get product with invalid Id {id}", id);
            throw new ArgumentOutOfRangeException(nameof(id), id, "Attempting to get product with invalid Id");
        }

        logger.LogInformation("Fetching product with id: {id}",id);

        var product = await productRepository.GetByIdAsync(id);
        return product ?? throw new NotFoundException(nameof(product), id);
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        logger.LogInformation("Updating product {ProductId}", product.Id);

        var result = await productRepository.UpdateAsync(product);

        if (result == null)
        {
            logger.LogWarning("Attempted to update non-existent product {ProductId}", product.Id);
            throw new NotFoundException(nameof(Product), product.Id);
        }

        logger.LogInformation("Product {ProductId} updated successfully", result.Id);
        return result;
    }
}
