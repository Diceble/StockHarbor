using Microsoft.EntityFrameworkCore;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.enums;
using StockHarbor.TenantApi.Persistence;

namespace StockHarbor.TenantApi.Repositories;

public class TenantRepository(TenantDbContext context) : ITenantRepository
{

    public async Task<Tenant?> GetActiveByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TenantId == tenantId && t.Status == TenantStatus.Active,cancellationToken);
    }

    public async Task<IReadOnlyList<Tenant>> GetByIdsAsync(List<Guid> tenantIds, CancellationToken cancellationToken = default)
    {
        return await context.Tenants
            .AsNoTracking()
            .Where(t => tenantIds.Contains(t.TenantId))
            .ToListAsync(cancellationToken);
    }

    public async Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await context.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
    }

    public async Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Tenant?> AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await context.Tenants.AddAsync(tenant, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return tenant;
    }

    public async Task UpdateTenantStatus(Guid tenantId, TenantStatus tenantStatus, CancellationToken ct = default)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.TenantId == tenantId,ct);
        if (tenant == null)
            return;

        tenant.Status = tenantStatus;

        context.Tenants.Update(tenant);

        await context.SaveChangesAsync(ct);
    }

    public void Update(Tenant tenant)
    {
        context.Tenants.Update(tenant);
    }

    public void Delete(Tenant tenant)
    {
        context.Tenants.Remove(tenant);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
