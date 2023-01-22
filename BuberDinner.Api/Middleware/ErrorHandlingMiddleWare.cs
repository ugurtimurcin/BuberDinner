using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace BuberDinner.Api.Middleware;

public class ErrorHandlingMiddleWare
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = JsonSerializer.Serialize(new { error = "An error occured while processing your request." });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}
