using Microsoft.Extensions.Caching.Memory;
using StockHarbor.Domain.Exceptions;
using StockHarbor.Domain.Interfaces.Provider;
using StockHarbor.Domain.Models;

namespace StockHarbor.Infrastructure.Providers;
public class TenantProvider : ITenantProvider
{
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;

    public TenantProvider(IMemoryCache cache, HttpClient httpClient)
    {
        _cache = cache;
        _httpClient = httpClient;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    /// <exception cref="TenantNotResolvedException"></exception>
    public async Task<TenantInfo> GetTenantInfoAsync(string tenantId)
    {
        var tentantInfo = await _cache.GetOrCreateAsync($"tenant_{tenantId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);

            //TODO: make call right
            //var response = await _httpClient.GetAsync($"https://identity-service/api/tenants/{tenantId}");
            //if (!response.IsSuccessStatusCode)
            //    throw new TenantNotResolvedException(tenantId);

            //var dto = await response.Content.ReadFromJsonAsync<TenantInfo>();

            var dto = new TenantInfo() { 
                DisplayName = "StockHarbor",
                TenantId = Guid.Parse("E6BD2252-138E-41B1-8098-D308F7054D08"),
                ConnectionString = "Host=localhost;Port=5432;Database=StockHarbor.ApiDb;Username=StockHarbor;Password=StockHarborPassword" };

            return dto == null
                ? null
                : new TenantInfo
            {
                TenantId = dto.TenantId,
                DisplayName = dto.DisplayName,
                ConnectionString = dto.ConnectionString
            };
        });

        return tentantInfo ?? throw new TenantNotResolvedException(tenantId);
    }
}