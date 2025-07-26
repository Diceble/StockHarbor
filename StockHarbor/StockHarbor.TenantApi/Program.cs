
using FastEndpoints;
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
        builder.Services.AddFastEndpoints();

        builder.Services.AddDbContext<TenantDbContext>(options => 
            options.UseNpgsql(builder.Configuration.GetConnectionString("TenantDb")));


        var app = builder.Build();


        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints();

        app.Run();
    }
}
