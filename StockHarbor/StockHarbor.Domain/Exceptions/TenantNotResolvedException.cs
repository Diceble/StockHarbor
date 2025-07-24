namespace StockHarbor.Domain.Exceptions;

public class TenantNotResolvedException : Exception
{
    public TenantNotResolvedException()
        : base("Tenant information could not be resolved from the current request.") { }

    public TenantNotResolvedException(string tenantId)
        : base($"Tenant '{tenantId}' could not be resolved from the current request.") { }
}

