using Microsoft.Azure.Cosmos;

namespace Funfair.Dal.CosmosDb.Linq;

public static class LinqMethod
{
    public static async Task<T?> FirstOrDefaultAsync<T>(this FeedIterator<T> feedIterator,
        CancellationToken cancellationToken = default)
    {
        if (feedIterator.HasMoreResults)
        {
            foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
            {
                return item;
            }
        }

        return default!;
    }
    public static async Task<List<T>> ToListAsync<T>(this FeedIterator<T> feedIterator,
        CancellationToken cancellationToken = default)
    {
        var result = new List<T>();
        
        if (feedIterator.HasMoreResults)
        {
            result.AddRange(await feedIterator.ReadNextAsync(cancellationToken));
        }

        return result;
    }
}