using Funfair.Dal.CosmosDb.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Dal.CosmosDb;

public static class Extensions
{
    public static WebApplicationBuilder AddCosmosDb(this WebApplicationBuilder builder, ContainerOptions containerOptions)
    {
        
        var options = builder.Configuration.GetSection("CosmosDb").Get<CosmosOptions>();
        
        ArgumentNullException.ThrowIfNull(options, "CosmosDb options not found in configuration");

        builder.Services
            .AddSingleton(options)
            .AddSingleton<CosmosClient>(_ => InitializeCosmosClientInstance(options))
            .AddSingleton<Database>(sp =>
                CreateDatabaseAsync(sp.GetRequiredService<CosmosClient>(), sp.GetRequiredService<CosmosOptions>())
                    .GetAwaiter().GetResult())
            .AddSingleton<Container>(sp =>
                CreateContainerAsync(sp.GetRequiredService<Database>(), containerOptions)
                    .GetAwaiter().GetResult());


        return builder;
    }
    
    
    private static CosmosClient InitializeCosmosClientInstance(CosmosOptions options)
    {
        var account = options.EndpointUri;
        var key = options.PrimaryKey;
        
        
        return new CosmosClient(account, key);
    }
    
    public static async Task<Database> CreateDatabaseAsync(CosmosClient client, CosmosOptions options)
    {
        var response = await client.CreateDatabaseIfNotExistsAsync(options.DatabaseId);

        return response.Database;

    }
    public static async Task<Container> CreateContainerAsync(Database database, ContainerOptions options)
    {
        var result = await  database.CreateContainerIfNotExistsAsync(options.ContainerId, options.PartitionKey);
        
        return result.Container;
    }
}