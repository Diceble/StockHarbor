namespace StockHarbor.Domain.Exceptions;
public abstract class StockHarborException : Exception
{
    protected StockHarborException(string message) : base(message) { }
    protected StockHarborException(string message, Exception innerException) : base(message, innerException) { }
}
