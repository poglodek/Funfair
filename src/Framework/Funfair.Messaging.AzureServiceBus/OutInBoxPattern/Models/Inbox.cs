using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

public class Inbox
{
    public int Id { get; set; }
    public DateTime DateReceived { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string MessageType { get; set; } = null!;
    
    [Column(TypeName = "varchar(MAX)")]
    [MaxLength]
    public string Message { get; set; } = null!;
    public Guid MessageId { get; init; }
    public string ErrorMessage { get; set; } = string.Empty;
}