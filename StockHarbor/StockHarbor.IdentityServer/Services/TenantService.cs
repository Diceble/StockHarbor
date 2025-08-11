using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

    public async Task SyncUsersForTenant(Guid tenantId, IEnumerable<string> selectedUserIds, CancellationToken ct = default)
    {
        var selected = new HashSet<string>(selectedUserIds ?? Array.Empty<string>(), StringComparer.Ordinal);

        var currentRows = await dbContext.Set<UserTenant>()
            .Where(ut => ut.TenantId == tenantId)
            .ToListAsync(ct);

        var current = new HashSet<string>(currentRows.Select(r => r.UserId), StringComparer.Ordinal);

        var toAddIds = selected.Except(current).ToArray();
        var toRemoveIds = current.Except(selected).ToArray();

        var toAdd = toAddIds.Select(uid => new UserTenant { TenantId = tenantId, UserId = uid }).ToList();
        var toRemove = currentRows.Where(r => toRemoveIds.Contains(r.UserId)).ToList();

        if (toAdd.Count > 0) dbContext.Set<UserTenant>().AddRange(toAdd);
        if (toRemove.Count > 0) dbContext.Set<UserTenant>().RemoveRange(toRemove);
        if (toAdd.Count > 0 || toRemove.Count > 0) await dbContext.SaveChangesAsync(ct);
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

    public async Task<bool> VerifyUserTenantAsync(string userId, string selectedTenantId)
    {
        return await dbContext.Set<UserTenant>()
            .AnyAsync(ut => ut.UserId == userId && ut.TenantId.ToString() == selectedTenantId);
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
