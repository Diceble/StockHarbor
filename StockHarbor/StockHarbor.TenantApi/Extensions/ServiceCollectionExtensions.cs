using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StockHarbor.TenantApi.Interfaces;
using StockHarbor.TenantApi.Repositories;

namespace StockHarbor.TenantApi.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITenantRepository, TenantRepository>();
        return services;
    }

    public static IServiceCollection AddAPIAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001"; // IdentityServer URL
                options.RequireHttpsMetadata = false; // Set to true in production
                options.Audience = "tenantapi"; // <-- Important: match your ApiResource name

                // Optional: Configure additional validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidAudiences = ["tenantapi"],
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("TenantReadAccess", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "tenantapi.read");
            })
            .AddPolicy("TenantWriteAccess", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "tenantapi.write");
            });
        return services;
    }
}
