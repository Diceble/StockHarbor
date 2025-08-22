using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;
using StockHarbor.API.Extensions;
using StockHarbor.API.Middleware;

namespace StockHarbor.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFrameworkServices();
        builder.Services.AddRepositories();
        builder.Services.AddInfrastructureServices();

        builder.Services.AddFastEndpointServices();
        builder.Services.AddAPIAuthentication();
        builder.Services.AddAccessTokenManagement();
        // Add services to the container.

        builder.Services.AddAPIAuthorization();

        // Configure Serilog
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        var app = builder.Build();

        app.UseMiddleware<TenantMiddleware>();
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseSerilogRequestLogging();
        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints()
            .UseSwaggerGen(uiConfig: ui =>
            {
                ui.OAuth2Client = new();
                // OAuth2 configuration
                ui.OAuth2Client.ClientId = "swagger-ui";
                ui.OAuth2Client.AppName = "Swagger UI for StockHarbor API";
                ui.OAuth2Client.UsePkceWithAuthorizationCodeGrant = true;
                ui.OAuth2Client.Scopes.Add("stockharbor.product.read");
                ui.OAuth2Client.Scopes.Add("stockharbor.product.write");
                ui.OAuth2Client.Scopes.Add("profile");
                ui.OAuth2Client.Scopes.Add("openid");
            });
        app.Run();
    }
}
