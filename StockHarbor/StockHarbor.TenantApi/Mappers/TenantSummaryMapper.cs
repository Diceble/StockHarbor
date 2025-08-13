using FastEndpoints;
using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Mappers;

public class TenantSummaryMapper : Mapper<EmptyRequest, GetTenantIdAndNameResponse, Tenant>
{
    public override GetTenantIdAndNameResponse FromEntity(Tenant e)
    {
        return new GetTenantIdAndNameResponse(e.TenantId, e.TenantName);
    }
}
