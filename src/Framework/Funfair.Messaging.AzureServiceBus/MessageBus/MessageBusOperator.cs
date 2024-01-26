using System.Text;
using Azure.Messaging.ServiceBus;
using Funfair.Messaging.AzureServiceBus.Exception;
using Funfair.Messaging.AzureServiceBus.Options;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Funfair.Messaging.AzureServiceBus.Services;

namespace Funfair.Messaging.AzureServiceBus.MessageBus;

public class MessageBusOperator : IMessageBusOperator
{
    private readonly MessageBusOptions _options;

    public MessageBusOperator(MessageBusOptions options)
    {
        _options = options;
    }
    public async Task Publish(MessageServiceBus messageServiceBus, CancellationToken cancellationToken)
    {
        var client = new ServiceBusClient(_options.ConnectionString);
        var busClient = client.CreateSender(messageServiceBus.MessageType);

        using var batch = await busClient.CreateMessageBatchAsync(cancellationToken);

        var canBeAdded = batch.TryAddMessage(new ServiceBusMessage
        {
            MessageId = messageServiceBus.MessageId.ToString(),
            Body = new BinaryData(Encoding.UTF8.GetBytes(messageServiceBus.Message)),
            ContentType = "application/json",
            Subject = messageServiceBus.MessageType
            
        });
        
        InvalidMessageSize.ThrowIfFalse(canBeAdded);
        
        await busClient.SendMessagesAsync(batch,cancellationToken);
    }
    
}