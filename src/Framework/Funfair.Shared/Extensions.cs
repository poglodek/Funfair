using System.Reflection;
using Funfair.Shared.App.Auth;
using Funfair.Shared.Core;
using Funfair.Shared.Core.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Shared;

public static class Extensions
{
    public static WebApplicationBuilder AddShared(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        builder.Services.AddTransient<IClock, ClockNow>();
        builder.Services.AddTransient<IUserContextAccessor, UserContextAccessor>();

        builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
        
        return builder;
    }


   
}