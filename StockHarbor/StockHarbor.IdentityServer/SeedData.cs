using System.Security.Claims;
using IdentityModel;
using StockHarbor.IdentityServer.Data;
using StockHarbor.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace StockHarbor.IdentityServer;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var admin = userMgr.FindByNameAsync("admin").Result;
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@stockharbor.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(admin, "Admin123!").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(admin, [
                new Claim(JwtClaimTypes.Name, "Admin User"),
                new Claim(JwtClaimTypes.GivenName, "Admin"),
                new Claim(JwtClaimTypes.FamilyName, "User"),
                new Claim(JwtClaimTypes.WebSite, "http://admin.stockharbor.com"),
                new Claim("role", "Administrator"),
                new Claim("warehouse_id", "ALL"),
                new Claim("department", "Management")
            ]).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("admin created");
        }
        else
        {
            Log.Debug("admin already exists");
        }
        // Create Warehouse Manager
        var manager = userMgr.FindByNameAsync("warehouse.manager").Result;
        if (manager == null)
        {
            manager = new ApplicationUser
            {
                UserName = "warehouse.manager",
                Email = "manager@stockharbor.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(manager, "Manager123!").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(manager, [
                new Claim(JwtClaimTypes.Name, "Warehouse Manager"),
                new Claim(JwtClaimTypes.GivenName, "Warehouse"),
                new Claim(JwtClaimTypes.FamilyName, "Manager"),
                new Claim("role", "WarehouseManager"),
                new Claim("warehouse_id", "WH001"),
                new Claim("department", "Operations")
            ]).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("warehouse manager created");
        }
        else
        {
            Log.Debug("warehouse manager already exists");
        }
    }
}
