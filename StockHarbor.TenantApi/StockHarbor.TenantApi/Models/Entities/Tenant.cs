namespace StockHarbor.TenantApi.Models.Entities;

public class Tenant
{
    public required Guid TenantId { get; set; }
    public required string TenantName { get; set; }
    public required string ConnectionString { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? ContactEmail { get; set; }
}
