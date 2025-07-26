using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Mappers;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Endpoints;

[HttpGet("/api/tenant")]
[Authorize(Policy = "TenantReadAccess")]
public class GetAllTenantsEndpoint(ITenantRepository tenantRepository) : EndpointWithoutRequest<IEnumerable<GetTenantResponse>, TenantMapper>
{

    public override async Task HandleAsync(CancellationToken ct)
    {
        var tenants = await tenantRepository.GetAllAsync(ct);

        if (tenants is null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }        
        await Send.OkAsync(tenants.Select(Map.FromEntity), ct);
    }

}

