using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Models.enums;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Endpoints;

[HttpGet("/api/tenant/{tenantId}")]
[Authorize(Policy = "TenantReadAccess")]
public class GetTenantInformation(ITenantRepository tenantRepository) : Endpoint<GetTenantRequest, GetTenantResponse>
{

    public override async Task HandleAsync(GetTenantRequest request, CancellationToken ct)
    {
        var tenant = await tenantRepository.GetActiveByIdAsync(request.TenantId);

        if (tenant is null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }            

    }
}
