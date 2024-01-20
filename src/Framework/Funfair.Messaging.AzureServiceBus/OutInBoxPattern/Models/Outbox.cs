using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

public class Outbox
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? SentDate { get; set; }
    public string MessageType { get; set; } = null!;
    
    public string Message { get; set; } = null!;
    public Guid MessageId { get; init; }
    public string? ErrorMessage { get; set; } = string.Empty;
    
}