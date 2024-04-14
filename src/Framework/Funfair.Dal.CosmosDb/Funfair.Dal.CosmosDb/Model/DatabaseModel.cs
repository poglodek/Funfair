namespace Funfair.Dal.CosmosDb.Model;

public class DatabaseModel<T>
{
    public Guid Id { get; set; }

    public DateTimeOffset Created { get; set; }
    public T Object { get; set; }
    public DateTimeOffset Updated { get; set; }
    public Guid Version { get; set; }
}