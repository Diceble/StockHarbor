using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace StockHarbor.Infrastructure.Persistance;
internal class StockHarborDesignTimeDatabaseContextFactory : IDesignTimeDbContextFactory<StockHarborDatabaseContext>
{
    public StockHarborDatabaseContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<StockHarborDatabaseContext>();

        builder.UseNpgsql("Host=localhost;Port=5432;Database=StockHarbor.ApiDb;;Username=StockHarbor;Password=StockHarborPassword");

        return new StockHarborDatabaseContext(builder.Options);
    }
}