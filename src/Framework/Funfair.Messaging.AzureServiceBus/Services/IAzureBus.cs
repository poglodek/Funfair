using Azure.Messaging.ServiceBus;

namespace Funfair.Messaging.AzureServiceBus.Services;

public interface IAzureBus
{
    ServiceBusProcessor CreateProcessor(string exchange);
    void CreateBus();
}