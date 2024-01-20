using Funfair.Dal.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern;

public class InBoxContainer : ContainerContext
{
    public InBoxContainer(Container container) : base(container)
    {
    }
}