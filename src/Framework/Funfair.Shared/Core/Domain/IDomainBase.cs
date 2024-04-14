using System.Collections.ObjectModel;
using Funfair.Shared.Core.Events;

namespace Funfair.Shared.Domain;

public interface IDomainBase
{
    public ReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearEvents();
}