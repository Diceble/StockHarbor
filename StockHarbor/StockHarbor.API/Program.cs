
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using StockHarbor.API.Extensions;
using StockHarbor.Domain;
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

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseFastEndpoints()
            .UseSwaggerGen();
        // app.UseAuthorization();       
        app.Run();
    }
}
