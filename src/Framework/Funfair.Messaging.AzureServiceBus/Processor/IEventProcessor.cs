using Funfair.Messaging.AzureServiceBus.Events;

namespace Funfair.Messaging.AzureServiceBus.Processor;

public interface IEventProcessor
{
    public Task ProcessAsync(IEvent @event, CancellationToken token);
}