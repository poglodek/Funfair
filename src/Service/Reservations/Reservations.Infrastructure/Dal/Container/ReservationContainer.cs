using Funfair.Dal.CosmosDb;

namespace Reservations.Infrastructure.Dal.Container;

public class ReservationContainer(Microsoft.Azure.Cosmos.Container container) : ContainerContext(container);