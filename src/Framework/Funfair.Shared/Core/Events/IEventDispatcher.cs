using Funfair.Shared.Domain;

namespace Funfair.Shared.Core.Events;

public interface IEventDispatcher
{
    Task Publish(IDomainEvent @domainEvent, CancellationToken cancellationToken = default);
    Task Publish(IEnumerable<IDomainEvent> @domainEvents, CancellationToken cancellationToken = default);
    Task Publish(DomainBase domainBase, CancellationToken cancellationToken = default);
}