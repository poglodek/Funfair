using Funfair.Messaging.EventHubs.Events;

namespace Funfair.Messaging.EventHubs.Processor;

public interface IEventProcessor
{
    public Task ProcessAsync(IIntegrationEvent @event, CancellationToken token = default);
}