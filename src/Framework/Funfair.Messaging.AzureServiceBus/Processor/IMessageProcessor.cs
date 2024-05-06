using Funfair.Shared.App.Events;

namespace Funfair.Messaging.AzureServiceBus.Processor;

public interface IMessageProcessor
{
    public Task ProcessAsync(IIntegrationEvent @event, CancellationToken token = default);
}