using StockHarbor.IdentityServer.Data;
using StockHarbor.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StockHarbor.IdentityServer.Interfaces;
using StockHarbor.IdentityServer.Services;

namespace StockHarbor.IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
          configuration.ReadFrom.Configuration(context.Configuration)
              .WriteTo.Console()
              .WriteTo.File("logs/identityserver-.txt", rollingInterval: RollingInterval.Day));

        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential(); // Only for development;

        builder.Services.AddAuthentication();
        builder.Services.AddAccessTokenManagement();

        builder.Services.AddScoped<ITenantService, TenantService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddHttpClient<ITenantClient, TenantClient>(c =>
        {
            //c.BaseAddress = new Uri(builder.Configuration["TenantService:BaseUrl"]!);
            // add auth headers if needed
            c.BaseAddress = new Uri("https://localhost:7160/api");
        }).AddClientCredentialsTokenHandler("tenant-api");    

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        //app.UseCors("AllowSwaggerUI");

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }

    public static IServiceCollection AddAccessTokenManagement(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();

        services.AddClientCredentialsTokenManagement()
            .AddClient("tenant-api", client =>
            {
                client.TokenEndpoint = "https://localhost:5001/connect/token";
                client.ClientId = "identity-admin";
                client.ClientSecret = "super-secret";
                client.Scope = "tenantapi.read";                
            });

        return services;
    }
}