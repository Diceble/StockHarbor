using Microsoft.AspNetCore.Http;
using StockHarbor.Domain.Exceptions;
using StockHarbor.Domain.Interfaces.Resolver;
using StockHarbor.Domain.Models;

namespace StockHarbor.Infrastructure.Resolvers;
public class TenantResolver(IHttpContextAccessor httpContextAccessor) : ITenantResolver
{
    public TenantInfo GetCurrentTenant()
    {
        var context = httpContextAccessor.HttpContext;

        return context != null &&
            context.Items.TryGetValue("TenantInfo", out var tenantInfoObj) &&
            tenantInfoObj is TenantInfo tenantInfo
            ? tenantInfo
            : throw new TenantNotResolvedException();
    }
}
