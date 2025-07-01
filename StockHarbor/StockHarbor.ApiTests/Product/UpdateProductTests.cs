using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;

namespace StockHarbor.ApiTests.Product;
public class UpdateProductTests(StockHarborApiFixture App) : TestBase<StockHarborApiFixture>
{
    [Fact]
    public async Task UpdateProduct_WithProductVariants_ReturnSuccess()
    {
        await App.ClearDatabaseAsync();

        var createRequest = CreateProduct();
        var (createResponse, createResult) = await App.Client.POSTAsync<CreateProductRequest, CreateProductResponse>(
            "api/product/create", createRequest);

        createResponse.IsSuccessStatusCode.ShouldBeTrue();
        createResult.ProductName.ShouldBe("Test");
        createResult.ProductVariants.Count().ShouldBe(1);

        var request = UpdateProduct(createResult);
        var (rsp, result) = await App.Client.PUTAsync<UpdateProductRequest, UpdateProductResponse>(
            "api/product/", request);

        rsp.IsSuccessStatusCode.ShouldBeTrue();
        result.ProductName.ShouldBe("Updated Product Test");
        result.ProductVariants.Count().ShouldBe(1);
        result.ProductVariants.Any(v => v.Name == "UpdatedProductVariant").ShouldBeTrue();
    }

    private CreateProductRequest CreateProduct()
    {
        var productVariants = new List<CreateProductVariantRequest>()
        {
            new("Test-Variant", "Best test Variant", 2.5m, "EUR", "12345", Domain.Enums.ProductVariantStatus.Active),
        };

        var request = new CreateProductRequest("Test", productVariants);

        return request;
    }

    private UpdateProductRequest UpdateProduct(CreateProductResponse response)
    {
        var updatedProductVariants = new List<UpdateProductVariantRequest>() { };

        foreach (var variant in response.ProductVariants)
        {
            updatedProductVariants.Add(new(variant.ProductVariantId, "UpdatedProductVariant", variant.Description, variant.Price.Amount, variant.Price.Currency, variant.Sku, variant.Status, variant.ProductId));
        }

        var updatedProduct = new UpdateProductRequest(response.ProductId,"Updated Product Test", updatedProductVariants);

        return updatedProduct;
    }
} 
