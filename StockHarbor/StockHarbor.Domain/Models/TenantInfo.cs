namespace StockHarbor.Domain.Models;

public class TenantInfo
{
    public Guid TenantId { get; set; }
    public string DisplayName { get; set; }
    public string ConnectionString { get; set; }
}
