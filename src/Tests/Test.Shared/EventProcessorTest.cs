using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;

namespace Test.Shared;

public class EventProcessorTest : IEventDispatcher
{
    public List<IDomainEvent> DomainEvents { get; init; } = new();
    
    public Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        DomainEvents.Add(domainEvent);
        
        return Task.CompletedTask;
    }

    public Task Publish(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        DomainEvents.AddRange(domainEvents);
        
        return Task.CompletedTask;
    }

    public Task Publish(DomainBase domainBase, CancellationToken cancellationToken = default)
    {
        DomainEvents.AddRange(domainBase.DomainEvents);
        
        return Task.CompletedTask;
    }
}