using System.Collections.ObjectModel;
using Funfair.Shared.Core.Events;

namespace Funfair.Shared.Domain;

public interface IDomainBase
{
    Id Id { get;  }
    public ReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearEvents();
}