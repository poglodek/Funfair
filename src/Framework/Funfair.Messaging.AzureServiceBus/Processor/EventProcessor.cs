using System.Text.Json;
using Funfair.Messaging.AzureServiceBus.Events;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Funfair.Shared.Events;
using Microsoft.Extensions.Logging;

namespace Funfair.Messaging.AzureServiceBus.Processor;

internal class EventProcessor : IEventProcessor
{
    private readonly OutboxDbContext _dbContext;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(OutboxDbContext dbContext, ILogger<EventProcessor> logger)
    {
        _dbContext = dbContext;
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
    
    private Task ProcessEventAsync(object @event, CancellationToken token)
    {
        _logger.LogDebug($"Saving event to outbox: {@event.GetType().Name}");

        var outbox = new Outbox
        {
            Message = JsonSerializer.Serialize(@event),
            MessageType = @event.GetType().Name,
            CreatedDate = DateTime.Now,
            MessageId = Guid.NewGuid()
        };

        _dbContext.Outboxes.Add(outbox);

        _logger.LogDebug($"Saved event to outbox: {@event.GetType().Name}");

        return _dbContext.SaveChangesAsync(token);
    }
}