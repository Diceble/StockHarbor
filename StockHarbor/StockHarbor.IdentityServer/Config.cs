using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace StockHarbor.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                // Custom identity resource for WMS roles
                new("roles", "User roles", ["role"])
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("stockharbor.api", "StockHarbor WMS API")
            {
                    UserClaims = { "role", "warehouse_id", "department" }
            },
            new ApiScope("stockharbor.inventory", "Inventory Management"),
            new ApiScope("stockharbor.orders", "Order Management"),
            new ApiScope("stockharbor.reports", "Reporting Access")
        ];

    // API resources
    public static IEnumerable<ApiResource> ApiResources =>
        [
                new ApiResource("stockharbor", "StockHarbor WMS")
                {
                    Scopes = { "stockharbor.api", "stockharbor.inventory", "stockharbor.orders", "stockharbor.reports" },
                    UserClaims = { "role", "warehouse_id", "department" }
                }
        ];

    // Clients (applications that can request tokens)
    public static IEnumerable<Client> Clients =>
        [
                // Interactive ASP.NET Core Web App
                new Client
                {
                    ClientId = "stockharbor.web",
                    ClientSecrets = { new Secret("web-client-secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "stockharbor.api"
                    },

                    AllowOfflineAccess = true
                },
                
                // API Client (for service-to-service communication)
                new Client
                {
                    ClientId = "stockharbor.api.client",
                    ClientSecrets = { new Secret("api-client-secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "stockharbor.api" }
                },

                // Mobile/SPA Client (for warehouse mobile apps)
                new Client
                {
                    ClientId = "stockharbor.mobile",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "stockharbor://callback" },
                    PostLogoutRedirectUris = { "stockharbor://callback" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "stockharbor.inventory",
                        "stockharbor.orders"
                    },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },

                new Client
                {
                    ClientId = "swagger-ui",
                    ClientName = "Swagger UI for StockHarbor API",

                    AllowedGrantTypes = GrantTypes.Code, // Authorization Code flow
                    RequirePkce = true, // Use PKCE for security
                    RequireClientSecret = false, // Public client (Swagger UI)
            
                    // Redirect URIs for Swagger
                    RedirectUris =
                    {
                        "https://localhost:5000/swagger/oauth2-redirect.html",
                        "https://localhost:7083/swagger/oauth2-redirect.html" // If using different ports
                    },
            
                    // Allowed CORS origins
                    AllowedCorsOrigins =
                    {
                        "https://localhost:5000",
                        "https://localhost:7083"
                    },
            
                    // Scopes this client can request
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "stockharbor.api",
                        "stockharbor.inventory",
                        "stockharbor.orders",
                        "stockharbor.reports"
                    },
            
                    // Token lifetimes
                    AccessTokenLifetime = 3600, // 1 hour
                    RefreshTokenUsage = TokenUsage.ReUse,
            
                    // Allow offline access for refresh tokens
                    AllowOfflineAccess = true
                }
        ];
}
