using Gremlin.Net.Driver;

namespace Funfair.Dal.Gremlin.GermlinApi;

public class GremlinRepository(GremlinClient client) : IGremlinRepository
{
    public Task SubmitAsync(string query, Dictionary<string,object> parameters, CancellationToken cancellationToken = default)
    {
        return client.SubmitAsync(query, parameters, cancellationToken);
    }

    public Task<ResultSet<T>> SubmitAsync<T>(string query, Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        return client.SubmitAsync<T>(query, parameters, cancellationToken);
    }
}