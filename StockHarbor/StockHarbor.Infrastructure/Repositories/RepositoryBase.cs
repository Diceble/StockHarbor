using StockHarbor.Infrastructure.Persistance;

namespace StockHarbor.Infrastructure.Repositories;
public class RepositoryBase : IDisposable
{
    private readonly IStockHarborDatabaseContextFactory _dbContextFactory;
    private StockHarborDatabaseContext? _dbContext;

    protected RepositoryBase(IStockHarborDatabaseContextFactory databaseContextFactory)
    {
        _dbContextFactory = databaseContextFactory;
    }

    protected StockHarborDatabaseContext DbContext =>
        _dbContext ??= _dbContextFactory.Create();

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
