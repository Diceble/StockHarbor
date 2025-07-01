using Microsoft.AspNetCore.Mvc;

namespace StockHarbor.API.Models.Products.Request;

public class GetProductByIdRequest()
{
    [FromRoute]
    public int Id { get; set; }
}

