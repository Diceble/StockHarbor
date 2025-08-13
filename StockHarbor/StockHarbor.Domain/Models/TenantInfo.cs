namespace StockHarbor.Domain.Models;

public class TenantInfo
{
    public Guid TenantId { get; set; }
    public required string Name { get; set; }
    public required string ConnectionString { get; set; }
}
