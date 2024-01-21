using Funfair.Dal.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern;

public class InOutBoxContainer : ContainerContext
{
    public InOutBoxContainer(Container container) : base(container)
    {
    }
}