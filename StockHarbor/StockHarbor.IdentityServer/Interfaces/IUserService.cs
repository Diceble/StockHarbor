using StockHarbor.IdentityServer.Models.ViewModels;

namespace StockHarbor.IdentityServer.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<UserViewModel>> GetAllUsersAsync(CancellationToken ct);
    Task<IReadOnlyList<UserTenantViewModel>> GetAllUserTenantsAsync(CancellationToken ct);
    Task<UserViewModel?> GetUserByIdAsync(string userId, CancellationToken ct);    
}
