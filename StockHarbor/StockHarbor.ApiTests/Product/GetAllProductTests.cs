using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;

namespace StockHarbor.ApiTests.Product;
public class GetAllProductTests(StockHarborApiFixture App) : TestBase<StockHarborApiFixture>
{
    [Fact]
    public async Task GetAllProducts_WithProductVariants_ReturnSuccess()
    {        
        await App.ClearDatabaseAsync();
        await App.SeedAsync(true);

        var request = new GetAllProductRequest(true);
        var (rsp, result) = await App.Client.GETAsync<GetAllProductRequest, IEnumerable<ProductResponse>>(
            "api/product/", request);

        rsp.IsSuccessStatusCode.ShouldBeTrue();
        result.ShouldNotBeEmpty();
        result.ShouldAllBe(product => product.ProductVariants != null);
        result.ShouldAllBe(product => product.ProductVariants.Any());
    }

    [Fact]
    public async Task GetAllProducts_WithoutProductVariants_ReturnSuccess()
    {
        await App.ClearDatabaseAsync();
        await App.SeedAsync(true);
        var request = new GetAllProductRequest(false);
        var (rsp, result) = await App.Client.GETAsync<GetAllProductRequest, IEnumerable<ProductResponse>>(
            "api/product/", request);

        rsp.IsSuccessStatusCode.ShouldBeTrue();
        result.Count().ShouldBeGreaterThan(0);
        result.ShouldAllBe(product => product.ProductVariants != null);
        result.ShouldAllBe(product => !product.ProductVariants.Any());
    }

    [Fact]
    public async Task GetAllProducts_WithoutProducts_ReturnSuccess()
    {
        await App.ClearDatabaseAsync();
        var request = new GetAllProductRequest(true);
        var (rsp, result) = await App.Client.GETAsync<GetAllProductRequest, IEnumerable<ProductResponse>>(
            "api/product/", request);

        rsp.IsSuccessStatusCode.ShouldBeTrue();
        result.ShouldBeEmpty();
    }
}
