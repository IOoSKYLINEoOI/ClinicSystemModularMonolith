using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystemModularMonolith.Infrastructure.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ProblemDetails problemDetails;
        int statusCode;

        switch (exception)
        {
           /* case NotFoundException notFound:
                statusCode = StatusCodes.Status404NotFound;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Not Found",
                    Detail = notFound.Message
                };
                break; */

            case ValidationException validation:
                statusCode = StatusCodes.Status400BadRequest;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Validation Error",
                    Detail = validation.Message
                };
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Server Error",
                    Detail = exception.Message
                };
                break;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}
