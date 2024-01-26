using Funfair.Messaging.AzureServiceBus.MessageBus;
using Funfair.Messaging.AzureServiceBus.Options;
using Funfair.Messaging.AzureServiceBus.Processor;
using Funfair.Messaging.AzureServiceBus.Services;
using Funfair.Messaging.AzureServiceBus.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Messaging.AzureServiceBus;

public static class Extensions
{
     public static WebApplicationBuilder AddMessageBus(this WebApplicationBuilder builder)
     {
         var messageBusOptions = builder.Configuration.GetSection("AzureMessageBus").Get<MessageBusOptions>();
         
         builder
             .Services
                    .AddScoped<IMessageProcessor, MessageProcessor>()
                    .AddTransient<IMessageBusOperator, MessageBusOperator>()
                    .AddSingleton<IAzureBus, AzureBus>()
                    .AddSingleton<IAzureProcessor,AzureProcessor>();
         
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