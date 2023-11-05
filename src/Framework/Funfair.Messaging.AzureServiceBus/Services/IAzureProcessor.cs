namespace Funfair.Messaging.AzureServiceBus.Services;

public interface IAzureProcessor
{
    Task StartProcessing();
}