using StockHarbor.IdentityServer.Interfaces;
using StockHarbor.IdentityServer.Models.DTO;

namespace StockHarbor.IdentityServer.Services;

public class TenantClient(HttpClient http) : ITenantClient
{
    public async Task<IReadOnlyList<TenantDto>> GetAllAsync(CancellationToken ct)
    {
        var url = $"{http.BaseAddress}/tenant";

        var response = await http.GetFromJsonAsync<IReadOnlyList<TenantDto>>(url, ct);
        return response ?? [];
    }

    public async Task<IReadOnlyList<TenantDto>> GetByTenantIdsAsyc(IEnumerable<Guid> tenantIds, CancellationToken ct)
    {
        if (tenantIds == null || !tenantIds.Any())
            return [];

        string query = string.Join("&", tenantIds.Select(g => $"tenantIds={g}"));

        var url = $"{http.BaseAddress}/tenants/summary?{query}";
        var response = await http.GetFromJsonAsync<IReadOnlyList<TenantDto>>(url, ct);
        return response ?? [];
    }
}
