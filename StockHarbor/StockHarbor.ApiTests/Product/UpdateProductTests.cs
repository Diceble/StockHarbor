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
        createResult.Name.ShouldBe("Test");

        var request = UpdateProduct(createResult);
        var (rsp, result) = await App.Client.PUTAsync<UpdateProductRequest, UpdateProductResponse>(
            "api/product/", request);

        rsp.IsSuccessStatusCode.ShouldBeTrue();
        result.Name.ShouldBe("Updated Product Test");
    }

    private static CreateProductRequest CreateProduct()
    {
        var request = new CreateProductRequest("Test","description", "12345", Domain.Enums.ProductStatus.Active, Domain.Enums.ProductType.Stock);

        return request;
    }

    private static UpdateProductRequest UpdateProduct(CreateProductResponse response)
    {
        var updatedProduct = new UpdateProductRequest(response.ProductId,"Updated Product Test", "description", "12345", Domain.Enums.ProductStatus.Active, Domain.Enums.ProductType.Stock);

        return updatedProduct;
    }
} 
