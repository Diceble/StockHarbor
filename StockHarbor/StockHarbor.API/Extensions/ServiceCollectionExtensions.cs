using StockHarbor.Domain.Interfaces.Services;
using StockHarbor.Infrastructure.Services;

namespace StockHarbor.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
