using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Mappers;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Endpoints;

[HttpPost("/api/tenant/")]
[Authorize(Policy = "TenantWriteAccess")]
public class CreateTenantEndpoint(ITenantRepository tenantRepository) : Endpoint<CreateTenantRequest,CreateTenantResponse, CreateTenantMapper>
{
    public override async Task HandleAsync(CreateTenantRequest request, CancellationToken ct)
    {
        var tenant = await tenantRepository.AddAsync(Map.ToEntity(request), ct);

        if (tenant is null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(tenant), ct);
    }

}
