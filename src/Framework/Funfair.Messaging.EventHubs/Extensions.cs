using Funfair.Dal.CosmosDb;
using Funfair.Dal.CosmosDb.Options;
using Funfair.Messaging.EventHubs.BackgroundWorkers;
using Funfair.Messaging.EventHubs.Options;
using Funfair.Messaging.EventHubs.OutInBoxPattern;
using Funfair.Messaging.EventHubs.Processor;
using Funfair.Messaging.EventHubs.Query;
using Funfair.Messaging.EventHubs.Services;
using Funfair.Messaging.EventHubs.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Messaging.EventHubs;

public static class Extensions
{
     public static WebApplicationBuilder AddMessageBus(this WebApplicationBuilder builder)
     {
         var hubsOptions = builder.Configuration.GetSection("EventHubsOptions").Get<EventHubsOptions>();
         
         builder
             .AddCosmosDb<InOutBoxContainer>(new ContainerOptions
             {
                 ContainerId = "inOutBox",
                 PartitionKey = "/messageType",
             })
                .Services
                    .AddScoped<IEventProcessor, EventProcessor>()
                    .AddTransient<IEventHubSender, EventHubSender>()
                    .AddSingleton<IEventHubProcessor, EventHubProcessor>()
                    .AddSingleton<IOutboxQuery,OutboxQuery>()
                    .AddSingleton<IInboxQuery,InboxQuery>()
                    .AddHostedService<OutboxWorker>()
                    .AddHostedService<InboxWorker>();
         
         return builder;
     }

     public static WebApplication UseMessageBus(this WebApplication app)
     {
         var scope = app.Services.CreateScope();

         scope.ServiceProvider.GetRequiredService<IEventHubProcessor>().StartProcessing();


         return app;
     }
}