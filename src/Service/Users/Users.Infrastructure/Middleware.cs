using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Users.App.Exceptions;
using Users.Core.Exceptions;

namespace Users.Infrastructure;

public class Middleware : IMiddleware
{
    private readonly ILogger<Middleware> _logger;

    public Middleware(ILogger<Middleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (CoreException ex)
        {
            _logger.LogWarning(ex.ErrorMessage);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.ErrorMessage);
        }
        catch (AppException ex)
        {
            _logger.LogWarning(ex.ErrorMessage);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.ErrorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}