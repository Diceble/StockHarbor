namespace StockHarbor.IdentityServer.Models;

public class UserTenant
{
    public required string UserId { get; set; }
    public required Guid TenantId { get; set; }
}
