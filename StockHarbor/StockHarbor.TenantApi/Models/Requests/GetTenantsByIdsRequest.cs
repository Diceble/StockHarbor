namespace StockHarbor.TenantApi.Models.Requests;

public record GetTenantsByIdsRequest(List<Guid> TenantIds);
