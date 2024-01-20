using Funfair.Dal.CosmosDb;
using Funfair.Dal.CosmosDb.Options;
using Funfair.Messaging.AzureServiceBus.BackgroundWorkers;
using Funfair.Messaging.AzureServiceBus.MessageBus;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.Processor;
using Funfair.Messaging.AzureServiceBus.Services;
using Funfair.Messaging.AzureServiceBus.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Messaging.AzureServiceBus;

public static class Extensions
{
     public static WebApplicationBuilder AddMessageBus(this WebApplicationBuilder builder)
     {
         var options = builder.Configuration.GetSection("AzureMessageBus").Get<ContainerOptions>();
         
         builder
             .AddCosmosDb(options)
             .Services
             .AddScoped<IEventProcessor, EventProcessor>()
             .AddScoped<IMessageBusOperator, MessageBusOperator>()
             .AddSingleton<IAzureBus, AzureBus>()
             .AddSingleton<IAzureProcessor,AzureProcessor>()
             .AddHostedService<OutboxWorker>()
             .AddHostedService<InboxWorker>();
         
         return builder;
     }

     public static WebApplication UseMessageBus(this WebApplication app)
     {
         var scope = app.Services.CreateScope();

         scope.ServiceProvider.GetRequiredService<IAzureBus>().CreateBus();
         scope.ServiceProvider.GetRequiredService<IAzureProcessor>().StartProcessing().GetAwaiter().GetResult();
         
         
         return app;
     }
}