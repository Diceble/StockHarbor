namespace StockHarbor.TenantApi.Models.Requests;

public record CreateTenantRequest(string TenantName, string ConnectionString, string? ContactEmail);
