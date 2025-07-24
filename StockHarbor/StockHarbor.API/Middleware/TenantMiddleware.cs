using StockHarbor.Domain.Interfaces.Provider;

namespace StockHarbor.API.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;

        // Bypass middleware for Swagger, static files, etc.
        if (path != null && (
            path.StartsWith("/swagger")))
        {
            await _next(context);
            return;
        }

        var tenantProvider = context.RequestServices.GetRequiredService<ITenantProvider>();

        var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();

        if (string.IsNullOrEmpty(tenantId))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing Tenant ID");
            return;
        }

        var tenantInfo = await tenantProvider.GetTenantInfoAsync(tenantId);

        if (tenantInfo == null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Tenant not found");
            return;
        }

        // Store it in scoped service or HttpContext.Items
        context.Items["TenantInfo"] = tenantInfo;

        await _next(context);
    }
}
