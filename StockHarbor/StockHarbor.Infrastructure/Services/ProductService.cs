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

    public async Task<IEnumerable<Product>> GetAllProductsAsync(bool includeProductVariants)
    {
        return await _productRepository.GetAllAsync(includeProductVariants);
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        var updatedProduct = await _productRepository.UpdateAsync(product);

        if (updatedProduct != null)
        {
            ICollection<ProductVariant>? variants = null;
            if (product.Variants != null && product.Variants.Count != 0)
            {
                variants = [.. product.Variants];
                updatedProduct.Variants = await UpdateProductVariantRange(variants);
            }
        }
        return updatedProduct;
    }

    public async Task<ICollection<ProductVariant>> UpdateProductVariantRange(ICollection<ProductVariant> variants)
    {
        ICollection<ProductVariant> updatedVariants = [];
        foreach (var variant in variants)
        {
            var updatedVariant = await _productRepository.UpdateProductVariantAsync(variant);
            if (updatedVariant != null)
                updatedVariants.Add(updatedVariant);
        }
        return updatedVariants;
    }
}
