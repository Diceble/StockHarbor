using Microsoft.EntityFrameworkCore;
using StockHarbor.Domain.Entities;

namespace StockHarbor.Infrastructure.Extensions;
public static class ProductQueryExtensions
{
    public static IQueryable<Product> IncludeVariants(
        this IQueryable<Product> query,
        bool includeVariants)
    {
        return includeVariants
            ? query.Include(p => p.Variants)
            : query;
    }
}