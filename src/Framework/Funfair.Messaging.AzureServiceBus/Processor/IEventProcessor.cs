using Funfair.Messaging.AzureServiceBus.Events;
using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;


namespace Funfair.Messaging.AzureServiceBus.Processor;

public interface IEventProcessor
{
    public Task ProcessAsync(IIntegrationEvent @event, CancellationToken token = default);
    public Task ProcessAsync(IDomainEvent @event, CancellationToken token = default);
    public Task ProcessAsync(DomainBase domainBase, CancellationToken token = default);
}