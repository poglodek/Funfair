using Funfair.Dal.CosmosDb;
using Funfair.Dal.CosmosDb.Options;
using Funfair.Logging;
using Funfair.Messaging.EventHubs;
using Funfair.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Planes.App.Repositories;
using Planes.Infrastructure.Dal.Container;
using Planes.Infrastructure.Dal.Repositories;

namespace Planes.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var cosmosContainerOptions = builder.Configuration.GetSection("CosmosDb").Get<ContainerOptions>();
        
        
        builder
          .AddAzureLogAnalytics()
          .AddEventHubs()
          .AddCosmosDb<PlaneContainer>(cosmosContainerOptions)
          .AddShared()
          .Services
            .AddScoped<IPlaneRepository, PlaneRepository>()
            .AddScoped<SharedMiddleware>();

        return builder;
    }
    

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<SharedMiddleware>();
        app.UseEventBus();
        
        return app;
    }

}