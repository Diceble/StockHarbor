using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Entities;

namespace StockHarbor.ApiTests;

public class ProductTests(StockHarborApiFixture App) : TestBase<StockHarborApiFixture>
{
    [Fact]
    public async Task CreateProduct_ReturnSuccess()
    {
        var productVariants = new List<CreateProductVariantRequest>()
        {
            new("Test-Variant", "Best test Variant", 2.5m, "EUR", "12345", Domain.Enums.ProductVariantStatus.Active),
        };

        var request = new CreateProductRequest("Test", productVariants);
        var (rsp, result) = await App.Client.POSTAsync<CreateProductRequest, CreateProductResponse>(
            "api/product/create", request);

        rsp.IsSuccessStatusCode.ShouldBeTrue();
        result.ProductName.ShouldBe("Test");
        result.ProductVariants.Count().ShouldBe(1);
    }

    [Fact]
    public async Task CreateProduct_ReturnBadRequest()
    {
        var productVariants = new List<CreateProductVariantRequest>();
        var request = new CreateProductRequest("Test", productVariants);
        var (rsp, result) = await App.Client.POSTAsync<CreateProductRequest, CreateProductResponse>(
            "api/product/create", request);

        rsp.IsSuccessStatusCode.ShouldBeFalse();
        rsp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
