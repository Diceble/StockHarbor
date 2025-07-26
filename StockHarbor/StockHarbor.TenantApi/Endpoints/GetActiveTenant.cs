using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Mappers;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Endpoints;

[HttpGet("/api/tenant/active/{tenantId}")]
[Authorize(Policy = "TenantReadAccess")]
public class GetActiveTenant(ITenantRepository tenantRepository) : Endpoint<GetTenantRequest, GetTenantResponse, TenantMapper>
{

    public override async Task HandleAsync(GetTenantRequest request, CancellationToken ct)
    {
        var tenant = await tenantRepository.GetActiveByIdAsync(request.TenantId,ct);

        if (tenant is null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }            

        await Send.OkAsync(Map.FromEntity(tenant),ct);
    }
}
