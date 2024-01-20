using System.Net;
using System.Text.Json;
using Funfair.Messaging.AzureServiceBus.Events;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Funfair.Shared.Core.Events;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Funfair.Messaging.AzureServiceBus.Processor;

internal class EventProcessor : IEventProcessor
{
    private readonly OutBoxContainer _outBoxContainer;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(OutBoxContainer outBoxContainer, ILogger<EventProcessor> logger)
    {
        _outBoxContainer = outBoxContainer;
        _logger = logger;
    }

    public Task ProcessAsync(IIntegrationEvent @event,CancellationToken token)
    {
        return ProcessEventAsync(@event, token);
    }
    

    public Task ProcessAsync(IDomainEvent @event, CancellationToken token)
    {
        return ProcessEventAsync(@event, token);
    }
    
    private async Task ProcessEventAsync(object @event, CancellationToken token)
    {
        _logger.LogDebug($"Saving event to outbox: {@event.GetType().Name}");

        var outbox = new Outbox
        {
            Message = JsonSerializer.Serialize(@event),
            MessageType = @event.GetType().Name,
            CreatedDate = DateTime.Now,
            MessageId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        
        var result = await  _outBoxContainer.Container.CreateItemAsync(outbox, new PartitionKey(outbox.MessageType),
            cancellationToken: token);

        if (result.StatusCode == HttpStatusCode.Created)
        {
            _logger.LogDebug($"Saved event to outbox: {@event.GetType().Name}");
        }
            
        
    }
}