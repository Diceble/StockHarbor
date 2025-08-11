using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.EntityFrameworkCore;
using StockHarbor.IdentityServer.Data;
using StockHarbor.IdentityServer.Models;
using System.Security.Claims;

namespace StockHarbor.IdentityServer.Services;

public class TenantProfileService(ApplicationDbContext dbContext) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject?.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(sub)) return;

        var tenantIds = await dbContext.Set<UserTenant>()
         .Where(ut => ut.UserId == sub)
         .Select(ut => ut.TenantId.ToString())
         .OrderBy(x => x)
         .ToListAsync();

        if (tenantIds.Count == 0) return;

        var requested = new HashSet<string>(context.RequestedClaimTypes, StringComparer.Ordinal);

        if (requested.Contains("tenant_ids"))
        {
            var json = System.Text.Json.JsonSerializer.Serialize(tenantIds);
            context.IssuedClaims.Add(new Claim("tenant_ids", json, System.IdentityModel.Tokens.Jwt.JsonClaimValueTypes.JsonArray));
        }

        // Active tenant (single)
        if (requested.Contains("tenant_active"))
        {
            var active = context?.Subject?.FindFirst("tenant_active")?.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(active))
                context?.IssuedClaims.Add(new Claim("tenant_active", active));
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true; 
        return Task.CompletedTask;
    }
}
