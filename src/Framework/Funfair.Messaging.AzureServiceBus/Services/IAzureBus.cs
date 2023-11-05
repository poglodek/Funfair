using Azure.Messaging.ServiceBus;

namespace Funfair.Messaging.AzureServiceBus.Services;

public interface IAzureBus
{
    ValueTask<ServiceBusMessageBatch> CreateBatchAsync(string topic);
    Task SendAsync(string topic, ServiceBusMessageBatch batch);
}