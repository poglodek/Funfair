using Funfair.Shared.Domain;
using MediatR;

namespace Funfair.Shared.Core.Events;

internal class EventDispatcher(Mediator mediator) : IEventDispatcher
{

    public Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        return mediator.Publish(domainEvent, cancellationToken);
    }

    public  Task Publish(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        var tasks = domainEvents.Select(domainEvent => Publish(domainEvent, cancellationToken));
        return Task.WhenAll(tasks);
    }

    public Task Publish(DomainBase domainBase, CancellationToken cancellationToken = default)
    {
        return Publish(domainBase.DomainEvents, cancellationToken);
    }
}