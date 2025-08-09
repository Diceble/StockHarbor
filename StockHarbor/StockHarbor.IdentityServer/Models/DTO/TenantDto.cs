namespace StockHarbor.IdentityServer.Models.DTO;

public sealed class TenantDto
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = default!;
}