using Microsoft.Extensions.Logging;
using StockHarbor.Domain.Entities;
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
        var result = await productRepository.AddAsync(product);
        return result;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await productRepository.GetAllAsync();
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var updatedProduct = await productRepository.UpdateAsync(product);
        return updatedProduct;
    }
}
