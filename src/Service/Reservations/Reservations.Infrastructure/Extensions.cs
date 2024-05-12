using Funfair.Dal.CosmosDb;
using Funfair.Dal.CosmosDb.Options;
using Funfair.Logging;
using Funfair.Messaging.EventHubs;
using Funfair.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Infrastructure.Dal.Container;
using Reservations.Infrastructure.Dal.Repositories;

namespace Reservations.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var cosmosContainerOptions = builder.Configuration.GetSection("CosmosDb").Get<ContainerOptions>();
        
        
        builder
            .AddAzureLogAnalytics()
            .AddEventHubs()
            .AddCosmosDb<ReservationContainer>(cosmosContainerOptions)
            .AddShared()
            .Services
            .AddScoped<IReservationRepository, ReservationRepository>();

        return builder;
    }
    
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<SharedMiddleware>();
        app.UseEventBus();
        
        return app;
    }
}