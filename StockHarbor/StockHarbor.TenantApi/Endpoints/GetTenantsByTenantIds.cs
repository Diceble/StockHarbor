using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Mappers;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Endpoints;

[HttpGet("/api/tenants/summary")]
[Authorize(Policy = "TenantReadAccess")]

public class GetTenantsByTenantIds(ITenantRepository tenantRepository) : Endpoint<GetTenantsByIdsRequest, IReadOnlyList<GetTenantIdAndNameResponse>, TenantSummaryMapper>
{
    public override async Task HandleAsync(GetTenantsByIdsRequest request, CancellationToken ct)
    {
        var tenants = await tenantRepository.GetByIdsAsync(request.TenantIds, ct);
        if (tenants is null || !tenants.Any())
        {
            await Send.ForbiddenAsync(ct);
            return;
        }        

        var response = tenants.Select(t => Map.FromEntity(t)).ToList();
        await Send.OkAsync(response, ct);
    }
}