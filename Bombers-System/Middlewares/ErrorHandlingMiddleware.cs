using System.Net;
using System.Security.Authentication;
using Bombers_System.Domain.Exceptions;
using Newtonsoft.Json;

namespace Bombers_System.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                ConflictException => (HttpStatusCode.Conflict, exception.Message),
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                InvalidCredentialException => (HttpStatusCode.Unauthorized, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "Internal error ocurred.")
            };
            
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            var errorResponse = new { message = message };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}