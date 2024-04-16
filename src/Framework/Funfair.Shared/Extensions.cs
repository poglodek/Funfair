using System.Reflection;
using Funfair.Shared.App.Auth;
using Funfair.Shared.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Shared;

public static class Extensions
{
    public static WebApplicationBuilder AddPipelineBehavior(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        builder.Services.AddTransient<IClock, ClockNow>();
        builder.Services.AddTransient<IUserContextAccessor, UserContextAccessor>();
        
        return builder;
    }


   
}