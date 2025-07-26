using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.enums;

namespace StockHarbor.TenantApi.Models.Response;

public record GetTenantResponse(Guid TenantId, string DisplayName, string ConnectionString, TenantStatus TenantStatus, DateTime CreateDate, string? ContactEmail)
{
    public GetTenantResponse(Tenant tenant) : this(tenant.TenantId, tenant.TenantName, tenant.ConnectionString, tenant.Status,tenant.CreatedDate, tenant.ContactEmail) { }
}
