using Funfair.Messaging.EventHubs.OutInBoxPattern.Models;

namespace Funfair.Messaging.EventHubs.Services;

public interface IEventHubSender
{
    Task SendMessagesAsync(Outbox outbox, CancellationToken cancellationToken);
}