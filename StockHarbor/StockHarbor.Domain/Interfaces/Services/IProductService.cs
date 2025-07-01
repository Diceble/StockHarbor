using StockHarbor.Domain.Entities;

namespace StockHarbor.Domain.Interfaces.Services;
public interface IProductService
{
    Task<Product?> CreateProductAsync(Product request);
    Task<ProductVariant?> CreateProductVariantAsync(ProductVariant request);
    Task<IEnumerable<Product>> GetAllProductsAsync(bool includeProductVariants);
    Task<Product?> UpdateProductAsync(Product product);
    Task<ICollection<ProductVariant>> UpdateProductVariantRange(ICollection<ProductVariant> variants);
}
