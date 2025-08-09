using StockHarbor.IdentityServer.Models.DTO;

namespace StockHarbor.IdentityServer.Interfaces;

public interface ITenantService
{
    Task<IReadOnlyList<TenantDto>> GetAllTenantsAsync(CancellationToken ct);
    Task AddUserToTentant(string userId, Guid tenantId, CancellationToken ct);
    Task AddUsersToTenant(IEnumerable<string> userIds, Guid tenantId, CancellationToken ct);
    Task RemoveUserFromTenant(string userId, Guid tenantId, CancellationToken ct);
}
