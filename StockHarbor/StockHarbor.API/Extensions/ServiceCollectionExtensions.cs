using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StockHarbor.Domain.Interfaces.Provider;
using StockHarbor.Domain.Interfaces.Resolver;
using StockHarbor.Domain.Interfaces.Services;
using StockHarbor.Infrastructure.Persistance;
using StockHarbor.Infrastructure.Providers;
using StockHarbor.Infrastructure.Resolvers;
using StockHarbor.Infrastructure.Services;

namespace StockHarbor.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // --- Database context factories ---
        services.AddScoped<IStockHarborDatabaseContextFactory, StockHarborContextDatabaseFactory>();

        // --- Domain services implemented in infrastructure ---
        services.AddScoped<IProductService, ProductService>();

        // --- Tenant resolution and metadata providers ---
        services.AddScoped<ITenantResolver, TenantResolver>();
        services.AddHttpClient<ITenantProvider, TenantProvider>()
            .AddClientCredentialsTokenHandler("tenantapi");

        return services;
    }

    public static IServiceCollection AddFrameworkServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        return services;
    }

    public static IServiceCollection AddAPIAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001"; // Your IdentityServer URL
                options.RequireHttpsMetadata = false; // Set to true in production
                options.Audience = "stockharbor"; // Your API resource name from IdentityServer config

                // Optional: Configure additional validation parameters
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidAudiences = ["stockharbor"],
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
        return services;
    }

    public static IServiceCollection AddAccessTokenManagement(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();

        services.AddClientCredentialsTokenManagement()
            .AddClient("tenantapi", client =>
            {
                client.TokenEndpoint = "https://localhost:5001/connect/token";
                client.ClientId = "stockharbor.api";
                client.ClientSecret = "supersecret";
                client.Scope = "tenantapi.read";
            });

        return services;
    }

    public static IServiceCollection AddFastEndpointServices(this IServiceCollection services)
    {

        services.AddFastEndpoints()
        .SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "StockHarbor API";
                s.Version = "v1";
                s.Description = "StockHarbor Warehouse Management System API";
            };

            // Configure OAuth2 Authorization Code flow
            o.EnableJWTBearerAuth = false; // Disable default JWT config since we're using OAuth2

            o.DocumentSettings = s =>
            {
                s.Title = "StockHarbor API";
                s.Version = "v1";

                // Add OAuth2 Security Definition
                s.AddAuth("OAuth2", new()
                {
                    Type = NSwag.OpenApiSecuritySchemeType.OAuth2,
                    Flows = new NSwag.OpenApiOAuthFlows
                    {
                        AuthorizationCode = new NSwag.OpenApiOAuthFlow
                        {
                            AuthorizationUrl = "https://localhost:5001/connect/authorize",
                            TokenUrl = "https://localhost:5001/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                { "stockharbor.api", "StockHarbor API access" },
                                { "openid", "OpenID Connect" },
                                { "profile", "Profile information" }
                            }                            
                        },
                    },                    
                });

                s.AddAuth("TenantIdHeader", new()
                {
                    Name = "X-Tenant-ID",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Description = "Tenant ID required for multi-tenant access"
                });

                s.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("TenantIdHeader"));
            };
        });
        return services;
    }
}
