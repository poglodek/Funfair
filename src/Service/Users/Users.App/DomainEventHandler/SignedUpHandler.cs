using Funfair.Messaging.AzureServiceBus.Processor;
using MediatR;
using Users.Core.Events;

namespace Users.App.DomainEventHandler;

internal sealed class SignedUpHandler : INotificationHandler<SignedUp>
{
    private readonly IEventProcessor _eventProcessor;

    public SignedUpHandler(IEventProcessor eventProcessor)
    {
        _eventProcessor = eventProcessor;
    }
    
    public Task Handle(SignedUp notification, CancellationToken cancellationToken)
    {
        return _eventProcessor.ProcessAsync(notification, cancellationToken);
    }
}