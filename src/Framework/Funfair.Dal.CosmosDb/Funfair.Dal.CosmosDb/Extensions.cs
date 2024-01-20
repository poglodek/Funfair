using Funfair.Dal.CosmosDb.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Dal.CosmosDb;

public static class Extensions
{
    public static WebApplicationBuilder AddCosmosDb<T>(this WebApplicationBuilder builder, ContainerOptions containerOptions) where T : ContainerContext
    {
        
        var options = builder.Configuration.GetSection("CosmosDb").Get<CosmosOptions>();
        
        ArgumentNullException.ThrowIfNull(options, "CosmosDb options not found in configuration");
  
        var client = InitializeCosmosClientInstance(options);
        var database = CreateDatabaseAsync(client, options).GetAwaiter().GetResult();
        var container = CreateContainerAsync(database, containerOptions).GetAwaiter().GetResult();

        builder.Services
            .AddSingleton(options)
            .AddSingleton(client)
            .AddSingleton(database)
            .AddSingleton(sp => (T)Activator.CreateInstance(typeof(T), container)!);

        return builder;
    }
    
    
    private static CosmosClient InitializeCosmosClientInstance(CosmosOptions options)
    {
        var account = options.EndpointUri;
        var key = options.PrimaryKey;
        
        
        return new CosmosClient(account, key);
    }

    private static async Task<Database> CreateDatabaseAsync(CosmosClient client, CosmosOptions options)
    {
        var response = await client.CreateDatabaseIfNotExistsAsync(options.DatabaseId);

        return response.Database;

    }

    private static async Task<Container> CreateContainerAsync(Database database, ContainerOptions options)
    {
        var result = await  database.CreateContainerIfNotExistsAsync(options.ContainerId, options.PartitionKey);
        
        return result.Container;
    }
}