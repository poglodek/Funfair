using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Funfair.Messaging.EventHubs.Events;
using Funfair.Messaging.EventHubs.Options;
using Funfair.Messaging.EventHubs.OutInBoxPattern.Models;

namespace Funfair.Messaging.EventHubs.Services.Implementation;

internal sealed class EventHubSender : IEventHubSender
{
    private readonly EventHubsOptions _hubsOptions;


    public EventHubSender(EventHubsOptions hubsOptions)
    {
        _hubsOptions = hubsOptions;
    }
    
    public async Task SendMessagesAsync(Outbox outbox, CancellationToken cancellationToken)
    {
        await using var producerClient = new EventHubProducerClient(_hubsOptions.ConnectionString, outbox.MessageType);
        using var eventBatch = await producerClient.CreateBatchAsync(cancellationToken);


        var body = new BinaryData(Encoding.UTF8.GetBytes(outbox.Message));

        var @event = new EventData
        {
            EventBody = body,
            MessageId = outbox.MessageId.ToString(),
            ContentType = EventConst.ContentType
        };
        @event.Properties.Add(EventConst.MessageType, outbox.MessageType);
        
        eventBatch.TryAdd(@event);
        await producerClient.SendAsync(eventBatch,cancellationToken);
    }
}