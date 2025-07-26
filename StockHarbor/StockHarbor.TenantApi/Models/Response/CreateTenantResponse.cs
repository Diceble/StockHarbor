using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.enums;

namespace StockHarbor.TenantApi.Models.Response;

public record CreateTenantResponse(Guid TenantId, string TenantName, string ConnectionString, TenantStatus TenantStatus, DateTimeOffset CreateDate, string? ContactEmail)
{
    public CreateTenantResponse(Tenant tenant): this (tenant.TenantId, tenant.TenantName, tenant.ConnectionString, tenant.Status, tenant.CreatedDate, tenant.ContactEmail) { }
}
