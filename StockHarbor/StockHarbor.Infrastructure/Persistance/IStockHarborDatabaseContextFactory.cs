namespace StockHarbor.Infrastructure.Persistance;
public interface IStockHarborDatabaseContextFactory
{
    StockHarborDatabaseContext Create();
}
