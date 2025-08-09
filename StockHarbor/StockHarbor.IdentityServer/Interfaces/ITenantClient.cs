using StockHarbor.IdentityServer.Models.DTO;

namespace StockHarbor.IdentityServer.Interfaces;

public interface ITenantClient
{
    Task<IReadOnlyList<TenantDto>> GetAllAsync(CancellationToken ct);
}
