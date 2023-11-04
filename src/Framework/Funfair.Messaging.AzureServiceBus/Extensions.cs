using Funfair.DAL.MsSql;
using Funfair.Messaging.AzureServiceBus.BackgroundWorkers;
using Funfair.Messaging.AzureServiceBus.MessageBus;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.Processor;
using Funfair.Messaging.AzureServiceBus.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Messaging.AzureServiceBus;

public static class Extensions
{
     public static WebApplicationBuilder AddMessageBus(this WebApplicationBuilder builder)
     {
         builder.Services
             .AddScoped<IAssembliesService, AssembliesService>()
             .AddScoped<IEventProcessor, EventProcessor>()
             .AddScoped<IMessageBusOperator, MessageBusOperator>()
             .AddHostedService<InboxWorker>()
             .AddHostedService<OutboxWorker>();

         builder.AddMsSql<OutboxDbContext>();

         return builder;
     }

     public static WebApplication UseMessageBus(this WebApplication app)
     {
         
         var scope = app.Services.CreateScope();
         scope.ServiceProvider.GetRequiredService<OutboxDbContext>().Database.Migrate();
         
         return app;
     }
}