using FastEndpoints;
using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Mappers;

public class TenantMapper : Mapper<GetTenantRequest, GetTenantResponse, Tenant>
{
    public override GetTenantResponse FromEntity(Tenant e)
    {
        return new GetTenantResponse(e);
    }
}
