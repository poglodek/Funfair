using Microsoft.Azure.Cosmos;

namespace Funfair.Dal.CosmosDb;

public class ContainerContext
{
    public readonly Container Container;

    public ContainerContext(Container container)
    {
        Container = container;
    }
}