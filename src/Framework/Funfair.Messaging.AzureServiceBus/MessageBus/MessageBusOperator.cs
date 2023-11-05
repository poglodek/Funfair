using System.Text;
using Azure.Messaging.ServiceBus;
using Funfair.Messaging.AzureServiceBus.BackgroundWorkers;
using Funfair.Messaging.AzureServiceBus.Exception;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Funfair.Messaging.AzureServiceBus.Services;

namespace Funfair.Messaging.AzureServiceBus.MessageBus;

public class MessageBusOperator : IMessageBusOperator
{
    private readonly IAzureBus _azureBus;

    public MessageBusOperator(IAzureBus azureBus)
    {
        _azureBus = azureBus;
    }
    public async Task Publish(Outbox outbox)
    {
        var batch = await _azureBus.CreateBatchAsync(outbox.MessageType);

        var canBeAdded = batch.TryAddMessage(new ServiceBusMessage
        {
            MessageId = outbox.MessageId.ToString(),
            Body = new BinaryData(Encoding.UTF8.GetBytes(outbox.Message)),
            ContentType = "application/json",
            Subject = outbox.MessageType
            
        });
        
        InvalidMessageSize.ThrowIfFalse(canBeAdded,outbox.Id);

        await _azureBus.SendAsync(outbox.MessageType,batch);
        
        
    }
    
}