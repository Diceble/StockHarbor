using StockHarbor.Domain.Models;

namespace StockHarbor.Domain.Interfaces.Resolver;
public interface ITenantResolver
{
    TenantInfo GetCurrentTenant();
}
