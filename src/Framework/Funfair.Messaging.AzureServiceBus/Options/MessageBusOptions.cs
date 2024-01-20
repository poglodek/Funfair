namespace Funfair.Messaging.AzureServiceBus.Options;

public class MessageBusOptions
{
    public string ConnectionString { get; init; }
    public string ContainerIdInbox { get; init; }
    public string ContainerIdOutbox { get; init; }
    public bool Enabled { get; init; }
}