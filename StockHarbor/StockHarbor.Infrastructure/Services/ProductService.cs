using StockHarbor.Domain.Entities;
using StockHarbor.Domain.Interfaces.Repository;
using StockHarbor.Domain.Interfaces.Services;

namespace StockHarbor.Infrastructure.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> CreateProductAsync(Product request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Variants == null || request.Variants.Count == 0)
            throw new ArgumentException("Product must have at least one variant", nameof(request));

        var product = await _productRepository.AddProductWithVariants(request);
        return product;

    }

    public Task<ProductVariant?> CreateProductVariantAsync(ProductVariant request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllProducts(bool includeProductVariants)
    {
        return _productRepository.GetAllAsync(includeProductVariants);
    }
}
