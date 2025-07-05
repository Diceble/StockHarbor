using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
        ConnectionString = _pgContainer.GetConnectionString();
    }


    protected override ValueTask SetupAsync()
    {
        // place one-time setup for the fixture here
        return ValueTask.CompletedTask;
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
        {
            options.UseNpgsql(ConnectionString);
        });

        // Optionally apply migrations here if not using EnsureCreated
        using var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StockHarborDatabaseContext>();

        try
        {
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration failed: {ex.Message}");
            throw;
        }
    }

    protected async override ValueTask TearDownAsync()
    {
        try
        {
            if (_pgContainer != null)
            {
                await _pgContainer.StopAsync();
                await _pgContainer.DisposeAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TearDown error: {ex.Message}");
        }
    }

    public async Task SeedAsync(bool seedProducts)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StockHarborDatabaseContext>();
        if (seedProducts && !await db.Products.AnyAsync())
            await SeedProductsAsync(db);
    }

    private async Task SeedProductsAsync(StockHarborDatabaseContext db)
    {
        int amountOfProductToAdd = 5;
        // Add common test data that multiple tests might need
        var products = new List<Domain.Entities.Product>();
        for (int i = 0; i < amountOfProductToAdd; i++)
        {
            products.Add(new Domain.Entities.Product()
            {
                Name = $"Test product {i}",
                Description = "Best test product",
                Sku = "12345",
                Status = Domain.Enums.ProductStatus.Active,
                ProductType = Domain.Enums.ProductType.Stock,
            });
        }

        db.Products.AddRange(products);
        await db.SaveChangesAsync();
    }

    public async Task ClearDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StockHarborDatabaseContext>();

        try
        {
            // Clear tables in correct order (respecting foreign key constraints)
            db.Products.RemoveRange(db.Products);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database clear failed: {ex.Message}");
            // Consider recreating the database if clearing fails
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
        }
    }
}
