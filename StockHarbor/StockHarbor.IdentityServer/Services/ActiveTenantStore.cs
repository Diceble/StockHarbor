using Microsoft.Extensions.Caching.Distributed;
using StockHarbor.IdentityServer.Interfaces;

namespace StockHarbor.IdentityServer.Services;

public class ActiveTenantStore : IActiveTenantStore
{
    private readonly IDistributedCache _cache;
    public ActiveTenantStore(IDistributedCache cache) => _cache = cache;

    private static string Key(string sub, string clientId) => $"active-tenant:{sub}:{clientId}";

    public Task SetAsync(string sub, string clientId, string tenantId, CancellationToken ct) =>
        _cache.SetStringAsync(Key(sub, clientId), tenantId,
            new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(8) }, ct);

    public Task<string?> GetAsync(string sub, string clientId, CancellationToken ct) =>
        _cache.GetStringAsync(Key(sub, clientId), ct);
}
