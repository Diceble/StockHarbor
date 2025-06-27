using StockHarbor.Domain.Interfaces.Repository;
using StockHarbor.Domain.Interfaces.Services;
using StockHarbor.Infrastructure.Repositories;
using StockHarbor.Infrastructure.Services;

namespace StockHarbor.API.Extensions;

public static class RepositoryCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        // Add more repositories here

        return services;
    }
}
