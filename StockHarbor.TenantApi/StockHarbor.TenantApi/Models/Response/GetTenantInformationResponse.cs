using StockHarbor.TenantApi.Models.enums;

namespace StockHarbor.TenantApi.Models.Response;

public record GetTenantInformationResponse(Guid TenantId, string DisplayName, string ConnectionString, TenantStatus TenantStatus);
