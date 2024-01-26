using Funfair.Messaging.AzureServiceBus.Events;

namespace Funfair.Messaging.AzureServiceBus.Processor;

public interface IMessageProcessor
{
    public Task ProcessAsync(IMessageEvent @event, CancellationToken token = default);
}