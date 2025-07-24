using StockHarbor.Domain.Models;

namespace StockHarbor.Domain.Interfaces.Provider;

public interface ITenantProvider
{
    Task<TenantInfo> GetTenantInfoAsync(string tenantId);
}
