using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using Bombers_System.Domain.Exceptions;

namespace Bombers_System.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error occurred: {Message}", exception.Message);

            var (statusCode, message) = exception switch
            {
                ConflictException => (HttpStatusCode.Conflict, exception.Message),
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                InvalidCredentialException => (HttpStatusCode.Unauthorized, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An internal error occurred.")
            };
            
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            
            var errorResponse = new { status = (int)statusCode, message = message };
 
            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}