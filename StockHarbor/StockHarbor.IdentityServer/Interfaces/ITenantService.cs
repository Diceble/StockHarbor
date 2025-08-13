using StockHarbor.IdentityServer.Models.DTO;
using StockHarbor.IdentityServer.Models.ViewModels;

namespace StockHarbor.IdentityServer.Interfaces;

public interface ITenantService
{
    Task<IReadOnlyList<TenantDto>> GetAllTenantsAsync(CancellationToken ct);
    Task AddUserToTentant(string userId, Guid tenantId, CancellationToken ct);
    Task AddUsersToTenant(IEnumerable<string> userIds, Guid tenantId, CancellationToken ct);
    Task RemoveUserFromTenant(string userId, Guid tenantId, CancellationToken ct);
    Task SyncUsersForTenant(Guid tenantId, IEnumerable<string> selectedUserIds, CancellationToken ct = default);
    Task<bool> VerifyUserTenantAsync(string userId, string selectedTenantId);
    Task<IReadOnlyList<TenantViewModel>> GetAllTenantsByUserId(string userId, CancellationToken ct = default);
}
