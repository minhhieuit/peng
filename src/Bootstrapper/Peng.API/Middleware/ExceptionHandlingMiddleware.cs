using System.Net;
using System.Text.Json;
using Peng.SharedKernel.Exceptions;

namespace Peng.API.Middleware;

internal class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, code, message) = exception switch
        {
            NotFoundException e => (HttpStatusCode.NotFound, "NotFound", e.Message),
            ForbiddenException e => (HttpStatusCode.Forbidden, "Forbidden", e.Message),
            ConflictException e => (HttpStatusCode.Conflict, "Conflict", e.Message),
            DomainException e => (HttpStatusCode.BadRequest, "DomainError", e.Message),
            _ => (HttpStatusCode.InternalServerError, "InternalServerError", "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new { code, message, traceId = context.TraceIdentifier };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
