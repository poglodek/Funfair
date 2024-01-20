using Funfair.Dal.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern;

public class OutBoxContainer : ContainerContext
{
    public OutBoxContainer(Container container) : base(container)
    {
    }
}