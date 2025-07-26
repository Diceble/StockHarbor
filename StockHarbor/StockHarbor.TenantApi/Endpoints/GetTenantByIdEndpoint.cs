using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Mappers;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Endpoints;

[HttpGet("/api/tenant/{tenantId}")]
[Authorize(Policy = "TenantReadAccess")]
public class GetTenantByIdEndpoint(ITenantRepository tenantRepository) : Endpoint<GetTenantRequest, GetTenantResponse, TenantMapper>
{
    public override async Task HandleAsync(GetTenantRequest request, CancellationToken ct)
    {
        var tenant = await tenantRepository.GetByIdAsync(request.TenantId, ct);

        if (tenant is null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }
        await Send.OkAsync(Map.FromEntity(tenant), ct);
    }
}
