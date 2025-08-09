using Microsoft.AspNetCore.Identity;
using StockHarbor.IdentityServer.Data;
using StockHarbor.IdentityServer.Interfaces;
using StockHarbor.IdentityServer.Models;
using StockHarbor.IdentityServer.Models.ViewModels;

namespace StockHarbor.IdentityServer.Services;

public class UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext) : IUserService
{
    public Task<IReadOnlyList<UserViewModel>> GetAllUsersAsync(CancellationToken ct)
    {
        var users = userManager.Users
            .Select(u => new UserViewModel
            {
                UserId = u.Id,
                UserName = u.UserName ?? "unknown username"
            })
            .ToList();
        return Task.FromResult<IReadOnlyList<UserViewModel>>(users);
    }

    public Task<UserViewModel?> GetUserByIdAsync(string userId, CancellationToken ct)
    {
        var user = userManager.Users
            .Select(u => new UserViewModel
            {
                UserId = u.Id,
                UserName = u.UserName ?? "unknown username"
            })
            .FirstOrDefault(u => u.UserId == userId);

        return user == null ? Task.FromResult<UserViewModel?>(null) : Task.FromResult<UserViewModel?>(user);
    }

    public Task<IReadOnlyList<UserTenantViewModel>> GetAllUserTenantsAsync(CancellationToken ct)
    {
        var UserTenants = dbContext.UserTenants
            .Select(ut => new UserTenantViewModel
            {
                UserId = ut.UserId,
                TenantId = ut.TenantId,
            })
            .ToList();

        return Task.FromResult<IReadOnlyList<UserTenantViewModel>>(UserTenants);
    }
}
