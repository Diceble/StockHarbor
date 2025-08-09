namespace StockHarbor.IdentityServer.Models.ViewModels;

public class UserTenantViewModel
{
    public required string UserId { get; set; } 
    public required Guid TenantId { get; set; } 
}
