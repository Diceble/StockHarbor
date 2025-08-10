namespace StockHarbor.IdentityServer.Interfaces;

public interface IActiveTenantStore
{
    Task SetAsync(string sub, string clientId, string tenantId, CancellationToken ct);
    Task<string?> GetAsync(string sub, string clientId, CancellationToken ct);
}
