using System.Reflection;
using Funfair.Shared.App.PipeLineBehavior;
using Funfair.Shared.Core.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Shared;

public static class Extensions
{
    public static WebApplicationBuilder AddPipelineBehavior<TContainer>(this WebApplicationBuilder builder) where TContainer : class, ICosmosUnitOfWork
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(UnitOfWorkPipeline<,>));
        });

        builder.Services.AddSingleton<ICosmosUnitOfWork>(sp => sp.GetRequiredService<TContainer>());

        return builder;
    }


   
}