using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

namespace Funfair.Messaging.AzureServiceBus.MessageBus;

public class MessageBusOperator : IMessageBusOperator
{
    public Task Publish(Outbox outbox)
    {
        //TODO: Implement
        return Task.CompletedTask;
    }
}