
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

        builder.Services.AddFastEndpoints()
            .SwaggerDocument();
        // Add services to the container.
        //builder.Services.AddAuthorization();

        // Configure Serilog
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseSerilogRequestLogging();
        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseFastEndpoints()
            .UseSwaggerGen();
        // app.UseAuthorization();       
        app.Run();
    }
}
