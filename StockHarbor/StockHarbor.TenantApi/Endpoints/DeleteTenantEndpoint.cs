using FastEndpoints;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Models.enums;
using StockHarbor.TenantApi.Models.Requests;

namespace StockHarbor.TenantApi.Endpoints;

[HttpDelete("api/tenant/{tenantId}")]
public class DeleteTenantEndpoint(ITenantRepository tenantRepository) : Endpoint<DeleteTenantRequest>
{
    public override async Task HandleAsync(DeleteTenantRequest request, CancellationToken ct)
    {
        await tenantRepository.UpdateTenantStatus(request.TenantId,TenantStatus.Deleted, ct);
        await Send.NoContentAsync(ct);
    }
}