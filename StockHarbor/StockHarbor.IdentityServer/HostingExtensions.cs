using Duende.IdentityServer;
using StockHarbor.IdentityServer.Data;
using StockHarbor.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
}