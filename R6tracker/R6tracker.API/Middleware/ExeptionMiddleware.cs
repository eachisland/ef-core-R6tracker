using R6tracker.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace R6tracker.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionMiddleware> logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (BadRequestException ex)
        {
            logger.LogWarning(ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = JsonSerializer.Serialize(new { message });
        await context.Response.WriteAsync(response);
    }
}