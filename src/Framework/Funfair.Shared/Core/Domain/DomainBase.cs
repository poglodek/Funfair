﻿using System.Collections.ObjectModel;
using Funfair.Shared.Core.Events;
using Newtonsoft.Json;

namespace Funfair.Shared.Domain;

public abstract class DomainBase
{
    [JsonProperty("id")]
    [JsonConverter(typeof(IdJsonConverter))]
    public Id Id { get; protected set; }
    
    
    [JsonIgnore]
    private readonly List<IDomainEvent> _domainEvents = new();

    [JsonIgnore]
    public ReadOnlyCollection<IDomainEvent> DomainEvents 
        => new List<IDomainEvent>(_domainEvents).AsReadOnly();



    protected void RaiseEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}