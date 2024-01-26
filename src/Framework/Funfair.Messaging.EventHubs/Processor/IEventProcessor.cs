using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;

namespace Funfair.Messaging.EventHubs.Processor;

public interface IEventProcessor
{
    public Task ProcessAsync(IDomainEvent @event, CancellationToken token = default);
    public Task ProcessAsync(DomainBase domainBase, CancellationToken token = default);
}