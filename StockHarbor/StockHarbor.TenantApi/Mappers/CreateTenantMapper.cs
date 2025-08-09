using FastEndpoints;
using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.enums;
using StockHarbor.TenantApi.Models.Requests;
using StockHarbor.TenantApi.Models.Response;

namespace StockHarbor.TenantApi.Mappers;

public class CreateTenantMapper : Mapper<CreateTenantRequest, CreateTenantResponse, Tenant>
{
    public override CreateTenantResponse FromEntity(Tenant e)
    {
        return new CreateTenantResponse(e);
    }

    public override Tenant ToEntity(CreateTenantRequest r)
    {
        return new Tenant()
        {
            TenantName = r.TenantName,
            ConnectionString = "connection-string-should-be",
            Status = TenantStatus.Pending,
            CreatedDate = DateTimeOffset.UtcNow,
            ContactEmail = r.ContactEmail
        };
    }
}
