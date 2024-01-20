namespace Funfair.Dal.CosmosDb.Options;

public class ContainerOptions
{
    public string PartitionKey { get; init; }
    public string ContainerId { get; init; }
}