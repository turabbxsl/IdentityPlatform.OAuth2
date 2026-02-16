using IdentityPlatform.Core.Common;
using System.Net;

namespace IdentityPlatform.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await WriteJsonResponseAsync(context,
                    Result<object>.Failure("Unauthorized. Token is missing or invalid.", (int)HttpStatusCode.Unauthorized),
                    HttpStatusCode.Unauthorized);
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var message = $"An unexpected system error occurred: {exception.Message}";
        var result = Result<object>.Failure(message, (int)HttpStatusCode.InternalServerError);

        return WriteJsonResponseAsync(context, result, HttpStatusCode.InternalServerError);
    }

    private static Task WriteJsonResponseAsync<T>(HttpContext context, Result<T> result, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsJsonAsync(result);
    }
}
