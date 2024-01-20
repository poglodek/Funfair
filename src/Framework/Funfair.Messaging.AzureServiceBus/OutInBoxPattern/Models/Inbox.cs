using Newtonsoft.Json;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

public class Inbox
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    public DateTime DateReceived { get; set; }
    
    [JsonProperty("dateProcessed")]
    public DateTime? DateProcessed { get; set; }
    public string MessageType { get; set; } = null!;
    public string Message { get; set; } = null!;
    public Guid MessageId { get; init; }
    
    [JsonProperty("errorMessage")]
    public string? ErrorMessage { get; set; } = string.Empty;
}