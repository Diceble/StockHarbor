using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StockHarbor.API.Extensions;
using StockHarbor.Infrastructure;

namespace StockHarbor.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRepositories();
        builder.Services.AddApplicationServices();
        builder.Services.AddDbContext<StockHarborDatabaseContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=StockHarbor;Username=StockHarbor;Password=StockHarborPassword"));

        builder.Services.AddFastEndpointServices();
        builder.Services.AddDuendeIdentityAuthentication();
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Configure Serilog
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        var app = builder.Build();

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
                ui.OAuth2Client.Scopes.Add("stockharbor.api");
                ui.OAuth2Client.Scopes.Add("profile");
                ui.OAuth2Client.Scopes.Add("openid");
            });
        app.Run();
    }
}
