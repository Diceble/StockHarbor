using StockHarbor.API.Models.Products.Request;
using StockHarbor.API.Models.Products.Response;
using StockHarbor.Domain.Entities;

namespace StockHarbor.ApiTests.Product;

public class CreateProductTests(StockHarborApiFixture App) : TestBase<StockHarborApiFixture>
{
    [Fact]
    public async Task CreateProduct_ReturnSuccess()
    {

        var request = new CreateProductRequest("Test","best test product","0123456",Domain.Enums.ProductStatus.Active);
        var (rsp, result) = await App.Client.POSTAsync<CreateProductRequest, CreateProductResponse>(
            "api/product/create", request);

        rsp.IsSuccessStatusCode.ShouldBeTrue();
        result.Name.ShouldBe("Test");
    }

    //[Fact]
    //public async Task CreateProduct_ReturnBadRequest()
    //{
    //    var request = new CreateProductRequest("Test", "best test product", "0123456", Domain.Enums.ProductStatus.Active);
    //    var (rsp, result) = await App.Client.POSTAsync<CreateProductRequest, CreateProductResponse>(
    //        "api/product/create", request);

    //    rsp.IsSuccessStatusCode.ShouldBeFalse();
    //    rsp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    //}
}
