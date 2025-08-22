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
                new("roles", "User roles", ["role", "tenant_ids"])
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("stockharbor.product.read", "StockHarbor WMS API Product Reading"),
            new ApiScope("stockharbor.product.write", "StockHarbor WMS API Product Writing"),
            new ApiScope("stockharbor.inventory", "Inventory Management"),
            new ApiScope("stockharbor.orders", "Order Management"),            
            new ApiScope("stockharbor.reports", "Reporting Access"),
            new ApiScope("tenantapi.read", "Read access to Tenant API"),
            new ApiScope("tenantapi.write", "Write access to Tenant API")
        ];

    // API resources
    public static IEnumerable<ApiResource> ApiResources =>
        [
                new ApiResource("stockharbor", "StockHarbor WMS API")
                {
                    Scopes = { "stockharbor.product.read", "stockharbor.product.write" },
                    UserClaims = { "role", "tenant_active", "tenant_ids" }
                },
                new ApiResource("tenantapi", "StockHarbor Tenant API")
                {
                    Scopes = { "tenantapi.read", "tenantapi.write" },
                    UserClaims = { "tenant_ids" } 
                }
        ];

    // Clients (applications that can request tokens)
    public static IEnumerable<Client> Clients =>
        [
                // StockHarbor Identity Web App (MVC)
                new Client {
                  ClientId = "identity-admin",
                  ClientSecrets = { new Secret("super-secret".Sha256()) },
                  AllowedGrantTypes = GrantTypes.ClientCredentials,
                  AllowedScopes = { "tenantapi.read" }                
                },

                // StockHarbor Next.js Web App (SPA)
                new Client
                {
                    ClientId = "stockharbor.nextjs",
                    ClientName = "StockHarbor Next.js Web App",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false, // Public client for SPA
    
                    RedirectUris =
                    {
                        "https://localhost:3000/api/auth/callback/duende-identity-server6", // Fixed this line
                        "http://localhost:3000/api/auth/callback/duende-identity-server6"   // Also add HTTP version
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:3000",
                        "https://localhost:3000/logged-out"
                    },
                    AllowedCorsOrigins =
                    {
                        "https://localhost:3000"
                    },
                    AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "stockharbor.product.read", 
                        "stockharbor.product.write"
                    ],
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 3600,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 3600, // 1 hour sliding window
                    RefreshTokenUsage = TokenUsage.ReUse, // Keep your existing setting
                },    
                        
                // API Client (for service-to-service communication)
                new Client
                {
                    ClientId = "stockharbor.api",
                    ClientName = "StockHarbor API",
                    ClientSecrets = { new Secret("supersecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "tenantapi.read" }
                },

                // Swagger UI client for StockHarbor API
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
                        "stockharbor.product.read",
                        "stockharbor.product.write"
                    },            
                    // Token lifetimes
                    AccessTokenLifetime = 3600, // 1 hour
                    RefreshTokenUsage = TokenUsage.ReUse,            
                    // Allow offline access for refresh tokens
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "tenantapi.swagger",
                    ClientName = "Tenant API Swagger UI",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false, // no secret for browser-based apps like Swagger

                    RedirectUris = { "https://localhost:7160/swagger/oauth2-redirect.html" }, // replace with your actual port
                    AllowedCorsOrigins = { "https://localhost:7160" },
                    PostLogoutRedirectUris = { "https://localhost:7160/swagger/" },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "tenantapi.read",
                        "tenantapi.write"
                    },

                    AllowAccessTokensViaBrowser = true
                }
    ];
}