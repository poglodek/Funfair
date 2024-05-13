using Funfair.Dal.Gremlin.Options;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.Dal.Gremlin;

public static class Extensions
{
    public static WebApplicationBuilder AddGremlinApi(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.GetSection("GremlinOptions").Get<GremlinOptions>();
        
        ArgumentNullException.ThrowIfNull(options, "GremlinOptions not found in configuration");
        
        var gremlinServer = new GremlinServer(options.Address, options.Port, enableSsl: options.UseSsl);
        var messageSerializer = new GraphSON2MessageSerializer();
        var gremlinClient = new GremlinClient(gremlinServer, messageSerializer);

        
        builder.Services
            .AddSingleton(gremlinClient)
            .AddSingleton(options);
        

        return builder;
    }
}