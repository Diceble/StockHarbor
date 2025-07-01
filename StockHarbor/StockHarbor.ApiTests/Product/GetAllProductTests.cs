using StockHarbor.API.Models.Products.Response;
using System.Net.Http.Json;

namespace StockHarbor.ApiTests.Product;
public class GetAllProductTests(StockHarborApiFixture App) : TestBase<StockHarborApiFixture>
{
    [Fact]
    public async Task GetAllProducts_WithProductVariants_ReturnSuccess()
    {
        await App.ClearDatabaseAsync();
        await App.SeedAsync(true);
        var result = await App.Client.GetFromJsonAsync<IEnumerable<ProductResponse>>("api/product/", TestContext.Current.CancellationToken);
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetAllProducts_WithoutProducts_ReturnSuccess()
    {
        await App.ClearDatabaseAsync();
        var result = await App.Client.GetFromJsonAsync<IEnumerable<ProductResponse>>("api/product/", TestContext.Current.CancellationToken);
        result.ShouldBeEmpty();
    }
}
