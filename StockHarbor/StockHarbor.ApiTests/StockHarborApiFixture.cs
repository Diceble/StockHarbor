using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockHarbor.API;
using StockHarbor.Infrastructure;
using Testcontainers.PostgreSql;

namespace StockHarbor.ApiTests;
public class StockHarborApiFixture : AppFixture<Program>
{
    private PostgreSqlContainer? _pgContainer;
    public string ConnectionString { get; private set; } = string.Empty;
    public StockHarborApiFixture()
    {
        _pgContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .WithCleanUp(true)
            .Build();

        _pgContainer.StartAsync().GetAwaiter().GetResult();
    }


    protected async override ValueTask SetupAsync()
    {
        // place one-time setup for the fixture here       
    }

    protected override void ConfigureApp(IWebHostBuilder builder)
    {
        // Additional app configuration if needed
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // do test service registration here
        var descriptor = services.SingleOrDefault(d =>
          d.ServiceType == typeof(DbContextOptions<StockHarborDatabaseContext>));
        if (descriptor != null)
            services.Remove(descriptor);
        
        // Add test DbContext with container connection string
        services.AddDbContext<StockHarborDatabaseContext>(options =>
            options.UseNpgsql(_pgContainer?.GetConnectionString()));

        // Optionally apply migrations here if not using EnsureCreated
        using var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StockHarborDatabaseContext>();
        db.Database.Migrate();
    }

    protected async override ValueTask TearDownAsync()
    {
        if (_pgContainer != null)
        {
            await _pgContainer.StopAsync();
            await _pgContainer.DisposeAsync();
        }
    }
}
