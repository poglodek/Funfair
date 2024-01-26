using Newtonsoft.Json;

namespace Funfair.Messaging.EventHubs.OutInBoxPattern.Models;

public class Inbox
{
    [JsonProperty("type")]
    public static string Type => "Inbox";
    
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("dateReceived")]
    public DateTime DateReceived { get; set; }
    
    [JsonProperty("dateProcessed")]
    public DateTime? DateProcessed { get; set; }
    
    
    [JsonProperty("messageType")]
    public string MessageType { get; set; } = null!;
    public string Message { get; set; } = null!;
    
    public Guid MessageId { get; init; }
    
    [JsonProperty("errorMessage")]
    public string? ErrorMessage { get; set; } = string.Empty;
}