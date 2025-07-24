using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain.Interfaces.Resolver;

namespace StockHarbor.Infrastructure.Persistance;
public class StockHarborContextDatabaseFactory(ITenantResolver tenantResolver) : IStockHarborDatabaseContextFactory
{
    public StockHarborDatabaseContext Create()
    {
        var tenantInfo = tenantResolver.GetCurrentTenant();

        var optionsBuilder = new DbContextOptionsBuilder<StockHarborDatabaseContext>();
        optionsBuilder.UseNpgsql(tenantInfo.ConnectionString);
        return new StockHarborDatabaseContext(optionsBuilder.Options);
    }
}
