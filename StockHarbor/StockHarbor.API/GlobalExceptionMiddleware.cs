using StockHarbor.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace StockHarbor.API;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred. Request: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            ValidationException => (StatusCodes.Status400BadRequest, exception.Message),
            ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, exception.Message),
            ArgumentNullException => (StatusCodes.Status400BadRequest,exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            _ => (StatusCodes.Status500InternalServerError, "An error occurred")
        };

        context.Response.StatusCode = response.Item1;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = response.Item2 }));
    }
}
