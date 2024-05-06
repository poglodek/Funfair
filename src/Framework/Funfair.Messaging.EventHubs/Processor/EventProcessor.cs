using System.Text.Json;
using Funfair.Messaging.EventHubs.Events;
using Funfair.Messaging.EventHubs.OutInBoxPattern;
using Funfair.Messaging.EventHubs.OutInBoxPattern.Models;
using Funfair.Shared.Core.Events;
using Funfair.Shared.Domain;

namespace Funfair.Messaging.EventHubs.Processor;

internal class EventProcessor : IEventProcessor
{
    private readonly InOutBoxContainer _outBoxContainer;

    public EventProcessor(InOutBoxContainer outBoxContainer)
    {
        _outBoxContainer = outBoxContainer;
    }

    public Task ProcessAsync(IIntegrationEvent @event, CancellationToken token = default)
    {
        return ProcessEventAsync(@event, token);
    }
    

    private Task ProcessEventAsync(object @event, CancellationToken token)
    {
        var outbox = new Outbox
        {
            Message = JsonSerializer.Serialize(@event),
            MessageType = @event.GetType().Name,
            CreatedDate = DateTime.Now,
            MessageId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        return _outBoxContainer.Container.CreateItemAsync(outbox,
            cancellationToken: token);
    }
}