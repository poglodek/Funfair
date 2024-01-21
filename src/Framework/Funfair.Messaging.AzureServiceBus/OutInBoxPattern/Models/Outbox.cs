using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

public class Outbox
{
    [JsonProperty("type")]
    public static string Type => "Outbox";
    
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("createdDate")]
    public DateTime CreatedDate { get; set; }
    
    [JsonProperty("sentDate")]
    public DateTime? SentDate { get; set; }
    public string MessageType { get; set; } = null!;
    
    public string Message { get; set; } = null!;
    public Guid MessageId { get; init; }
    
    [JsonProperty("errorMessage")]
    public string? ErrorMessage { get; set; } = string.Empty;
    
}