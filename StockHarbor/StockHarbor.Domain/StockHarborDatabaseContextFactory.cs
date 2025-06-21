using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace StockHarbor.Domain;
internal class StockHarborDatabaseContextFactory : IDesignTimeDbContextFactory<StockHarborDatabaseContext>
{
    public StockHarborDatabaseContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<StockHarborDatabaseContext>();

        builder.UseNpgsql("Host=localhost;Port=5432;Database=StockHarbor;Username=StockHarbor;Password=StockHarborPassword");

        return new StockHarborDatabaseContext(builder.Options);
    }
}
