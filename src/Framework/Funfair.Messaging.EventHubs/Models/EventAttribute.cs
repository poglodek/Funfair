using Funfair.Messaging.EventHubs.Exception;

namespace Funfair.Messaging.EventHubs.Models;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EventAttribute : Attribute
{
    public string Exchange { get; }

    public EventAttribute(string exchange)
    {
        if (string.IsNullOrWhiteSpace(exchange))
        {
            throw new InvalidExchangeException("Exchange cannot be null or empty");
        }
        Exchange = exchange;
    }
}