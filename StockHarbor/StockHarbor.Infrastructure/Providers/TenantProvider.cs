using Microsoft.Extensions.Caching.Memory;
using StockHarbor.Domain.Exceptions;
using StockHarbor.Domain.Interfaces.Provider;
using StockHarbor.Domain.Models;
using System.Net.Http.Json;

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

            var response = await _httpClient.GetAsync($"https://localhost:7160/api/tenant/active/{tenantId}");
           
            if (!response.IsSuccessStatusCode)
                throw new TenantNotResolvedException(tenantId);

            var dto = await response.Content.ReadFromJsonAsync<TenantInfo>();            

            return dto == null
                ? null
                : new TenantInfo
            {
                TenantId = dto.TenantId,
                Name = dto.Name,
                ConnectionString = dto.ConnectionString
            };
        });

        return tentantInfo ?? throw new TenantNotResolvedException(tenantId);
    }
}