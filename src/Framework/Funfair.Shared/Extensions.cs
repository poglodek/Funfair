using System.Reflection;
using Funfair.Shared.App.PipeLineBehavior;
using Funfair.Shared.Core.PipeLineBehavior;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Shared;

public static class Extensions
{
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(UnitOfWorkPipeline<,>));
            cfg.AddOpenBehavior(typeof(DomainEventPipeline<,>));
            
        });

        return builder;
    }


   
}