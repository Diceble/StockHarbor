using Microsoft.EntityFrameworkCore;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Models.Entities;
using StockHarbor.TenantApi.Models.enums;
using StockHarbor.TenantApi.Persistence;

namespace StockHarbor.TenantApi.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _context;

    public TenantRepository(TenantDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant?> GetActiveByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .FirstAsync(t => t.TenantId == tenantId && t.Status == TenantStatus.Active,cancellationToken);
    }

    public async Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
    }

    public async Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await _context.Tenants.AddAsync(tenant, cancellationToken);
    }

    public void Update(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
    }

    public void Delete(Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
