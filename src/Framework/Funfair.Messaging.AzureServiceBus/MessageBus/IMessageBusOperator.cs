﻿using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;

namespace Funfair.Messaging.AzureServiceBus.MessageBus;

public interface IMessageBusOperator
{
    public Task Publish(Outbox outbox);
}