using Funfair.Dal.CosmosDb;

namespace Users.Infrastructure.DAL.Container;

public class UserContainer : ContainerContext
{
    public UserContainer(Microsoft.Azure.Cosmos.Container container) : base(container)
    {
    }
}