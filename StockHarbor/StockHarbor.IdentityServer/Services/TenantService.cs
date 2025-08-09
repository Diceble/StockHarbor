using StockHarbor.IdentityServer.Data;
using StockHarbor.IdentityServer.Interfaces;
using StockHarbor.IdentityServer.Models;
using StockHarbor.IdentityServer.Models.DTO;
using StockHarbor.IdentityServer.Models.ViewModels;

namespace StockHarbor.IdentityServer.Services;

public class TenantService(ApplicationDbContext dbContext, ITenantClient tenantClient) : ITenantService
{
    public Task AddUserToTentant(string userId, Guid tenantId, CancellationToken ct)
    {
        dbContext.UserTenants.Add(new UserTenant
        {
            UserId = userId,
            TenantId = tenantId
        });
        return dbContext.SaveChangesAsync(ct);
    }

    public Task AddUsersToTenant(IEnumerable<string> userIds, Guid tenantId, CancellationToken ct)
    {
        var userTenants = userIds.Select(userId => new UserTenant
        {
            UserId = userId,
            TenantId = tenantId
        }).ToList();
        dbContext.UserTenants.AddRange(userTenants);
        return dbContext.SaveChangesAsync(ct);
    }

    public Task<IReadOnlyList<UserTenantViewModel>> GetAllUserTenantsAsync(CancellationToken ct)
    {
        return Task.FromResult<IReadOnlyList<UserTenantViewModel>>
            ([.. dbContext.UserTenants
                .Select(ut => new UserTenantViewModel
                {
                    UserId = ut.UserId,
                    TenantId = ut.TenantId,
                })]);
    }

    public Task RemoveUserFromTenant(string userId, Guid tenantId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<TenantDto>> GetAllTenantsAsync(CancellationToken ct)
    {
        return await tenantClient.GetAllAsync(ct);
    }
}
