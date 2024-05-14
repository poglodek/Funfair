using Gremlin.Net.Driver;

namespace Funfair.Dal.Gremlin.GermlinApi;

public interface IGremlinRepository
{
    Task SubmitAsync(string query, Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
    
    Task<ResultSet<T>> SubmitAsync<T>(string query, Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
}