using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

namespace Funfair.Messaging.AzureServiceBus.MessageBus;

public class MessageBusOperator : IMessageBusOperator
{
    public Task Send(Outbox outbox)
    {
        //TODO: Implement
        return Task.CompletedTask;
    }
}