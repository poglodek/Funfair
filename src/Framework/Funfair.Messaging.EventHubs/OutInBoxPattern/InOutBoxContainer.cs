using Funfair.Dal.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Funfair.Messaging.EventHubs.OutInBoxPattern;

public class InOutBoxContainer : ContainerContext
{
    public InOutBoxContainer(Container container) : base(container)
    {
    }
}