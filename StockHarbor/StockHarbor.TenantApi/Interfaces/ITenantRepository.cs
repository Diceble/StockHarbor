using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.enums;

namespace StockHarbor.TenantApi.Interfaces;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Tenant?> AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
    void Update(Tenant tenant);
    void Delete(Tenant tenant);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Tenant?> GetActiveByIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task UpdateTenantStatus(Guid tenantId, TenantStatus tenantStatus, CancellationToken ct = default);
}
