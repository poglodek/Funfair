using System.Collections.ObjectModel;
using Funfair.Shared.Core.Events;

namespace Funfair.Shared.Domain;

public abstract class DomainBase
{
    public Id Id { get; protected set; }
    
    private readonly List<IDomainEvent> _domainEvents = new();

    public ReadOnlyCollection<IDomainEvent> DomainEvents 
        => new List<IDomainEvent>(_domainEvents).AsReadOnly();



    protected void RaiseEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}