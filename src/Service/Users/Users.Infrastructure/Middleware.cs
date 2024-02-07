using Funfair.Shared.App;
using Funfair.Shared.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Users.App.Exceptions;
using Users.Core.Exceptions;

namespace Users.Infrastructure;

public class Middleware(ILogger<Middleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (CoreException ex)
        {
            logger.LogWarning(ex.ErrorMessage);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.ErrorMessage);
        }
        catch (AppException ex)
        {
            logger.LogWarning(ex.ErrorMessage);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.ErrorMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }

    }
}