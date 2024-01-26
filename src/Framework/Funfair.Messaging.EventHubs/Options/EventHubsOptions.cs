namespace Funfair.Messaging.EventHubs.Options;

public class EventHubsOptions
{
    public string ConnectionString { get; init; }
    public string BlobConnectionString { get; init; }
    public bool Enabled { get; init; }
}