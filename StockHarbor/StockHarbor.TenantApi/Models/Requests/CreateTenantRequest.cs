namespace StockHarbor.TenantApi.Models.Requests;

public record CreateTenantRequest(string TenantName, string? ContactEmail);
