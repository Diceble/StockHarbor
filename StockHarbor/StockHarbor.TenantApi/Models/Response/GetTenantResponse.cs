using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.enums;

namespace StockHarbor.TenantApi.Models.Response;

public record GetTenantResponse(Guid Id, string Name, string ConnectionString, TenantStatus TenantStatus, DateTimeOffset CreateDate, string? ContactEmail)
{
    public GetTenantResponse(Tenant tenant) : this(tenant.TenantId, tenant.TenantName, tenant.ConnectionString, tenant.Status,tenant.CreatedDate, tenant.ContactEmail) { }
}
