using System.Text.Json;
using Funfair.Messaging.AzureServiceBus.MessageBus;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Funfair.Shared.App.Events;

namespace Funfair.Messaging.AzureServiceBus.Processor;

internal class MessageProcessor : IMessageProcessor
{
    private readonly IMessageBusOperator _busOperator;

    public MessageProcessor(IMessageBusOperator busOperator)
    {
        _busOperator = busOperator;
    }

    public Task ProcessAsync(IIntegrationEvent @event,CancellationToken token = default)
    {
        return ProcessEventAsync(@event, token);
    }
    

    private Task ProcessEventAsync(object @event, CancellationToken token)
    {
        var message = new MessageServiceBus
        {
            Message = JsonSerializer.Serialize(@event),
            MessageType = @event.GetType().Name,
            MessageId = Guid.NewGuid(),
        };

        return _busOperator.Publish(message, token);
    }
}