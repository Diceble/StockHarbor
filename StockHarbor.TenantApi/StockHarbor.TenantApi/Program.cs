
using FastEndpoints;
using StockHarbor.TenantApi.Extensions;

namespace StockHarbor.TenantApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddAPIAuthentication();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddFastEndpoints();


        var app = builder.Build();


        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints();

        app.Run();
    }
}
