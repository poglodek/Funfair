using Funfair.Messaging.AzureServiceBus.Events;

namespace Users.Core.Events;

public record SignedUp(int Id) : IEvent;
