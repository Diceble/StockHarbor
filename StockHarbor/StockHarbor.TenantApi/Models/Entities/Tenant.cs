using StockHarbor.TenantApi.Models.enums;

namespace StockHarbor.TenantApi.Models.Entities;

public class Tenant
{
    public required Guid TenantId { get; set; }
    public required string TenantName { get; set; }
    public required string ConnectionString { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required TenantStatus Status { get; set; } 
    public string? ContactEmail { get; set; }
}
