
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using StockHarbor.TenantApi.Extensions;
using StockHarbor.TenantApi.Persistence;

namespace StockHarbor.TenantApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddAPIAuthentication();
        builder.Services.AddServices();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddFastEndpointServices();

        builder.Services.AddDbContext<TenantDbContext>(options => 
            options.UseNpgsql(builder.Configuration.GetConnectionString("TenantDb")));


        var app = builder.Build();


        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints()
        .UseSwaggerGen(uiConfig: ui =>
        {
            ui.OAuth2Client = new();
            // OAuth2 configuration
            ui.OAuth2Client.ClientId = "tenantapi.swagger";
            ui.OAuth2Client.AppName = "Swagger UI for Tenant API";
            ui.OAuth2Client.UsePkceWithAuthorizationCodeGrant = true;
            ui.OAuth2Client.Scopes.Add("tenantapi.read");
            ui.OAuth2Client.Scopes.Add("tenantapi.write");
            ui.OAuth2Client.Scopes.Add("profile");
            ui.OAuth2Client.Scopes.Add("openid");
        });

        app.Run();
    }
}
