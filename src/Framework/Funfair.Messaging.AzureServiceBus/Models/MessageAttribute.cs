using Funfair.Messaging.AzureServiceBus.Exception;

namespace Funfair.Messaging.AzureServiceBus.Models;

public sealed class MessageAttribute : Attribute
{
    public string Exchange { get; }

    public MessageAttribute(string exchange)
    {
        if (string.IsNullOrWhiteSpace(exchange))
        {
            throw new InvalidExchangeException("Exchange cannot be null or empty");
        }
        Exchange = exchange;
    }
}