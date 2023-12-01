﻿using Funfair.Messaging.AzureServiceBus.Models;
using Funfair.Shared.Events;

namespace Users.Core.Events;

public record SignedUp(Guid Id, string Email, string FirstName,string LastName) : IDomainEvent;
