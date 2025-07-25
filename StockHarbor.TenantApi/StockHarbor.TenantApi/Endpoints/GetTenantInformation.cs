using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.TenantApi.Models.enums;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Endpoints;

[HttpGet("/api/tenant/{tenantId}")]
[Authorize(Policy = "TenantReadAccess")]
public class GetTenantInformation : Endpoint<GetTenantInformationRequest, GetTenantInformationResponse>
{

    public override async Task HandleAsync(GetTenantInformationRequest request, CancellationToken ct)
    {
        var expectedTenantId = Guid.Parse("E6BD2252-138E-41B1-8098-D308F7054D08");

        if (request.TenantId == expectedTenantId)
        {
            var response = new GetTenantInformationResponse(Guid.Parse("E6BD2252-138E-41B1-8098-D308F7054D08"), "StockHarbor", "Host=localhost;Port=5432;Database=StockHarbor.ApiDb;Username=StockHarbor;Password=StockHarborPassword", TenantStatus.Active);
            await Send.OkAsync(response,ct);
        }
        else
        {
            await Send.ForbiddenAsync(ct);
        }
    }
}
