using Funfair.Messaging.AzureServiceBus.Events;
using Funfair.Shared.Events;

namespace Funfair.Messaging.AzureServiceBus.Processor;

public interface IEventProcessor
{
    public Task ProcessAsync(IIntegrationEvent @event, CancellationToken token);
    public Task ProcessAsync(IDomainEvent @event, CancellationToken token);
}