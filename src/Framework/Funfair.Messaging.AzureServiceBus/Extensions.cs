using Microsoft.AspNetCore.Builder;

namespace Funfair.Messaging.AzureServiceBus;

public static class Extensions
{
     public static WebApplicationBuilder AddMessageBus(this WebApplicationBuilder builder)
     {
         return builder;
     }
}