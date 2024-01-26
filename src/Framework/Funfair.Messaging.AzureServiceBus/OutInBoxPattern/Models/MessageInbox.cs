namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

public class MessageInbox
{
    public string MessageType { get; set; } = null!;
    public string Message { get; set; } = null!;
    
    public Guid MessageId { get; init; }
    
}