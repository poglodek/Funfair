using Funfair.Dal.CosmosDb;

namespace Planes.Infrastructure.Dal.Container;

internal class PlaneContainer(Microsoft.Azure.Cosmos.Container container) : ContainerContext(container);