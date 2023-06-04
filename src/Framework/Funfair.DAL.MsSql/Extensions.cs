using GraphQL;
using GraphQL.Server.Ui.Altair;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.DAL.MsSql;

public static class Extensions
{
    public static WebApplicationBuilder AddMsSql<T,T2>(this WebApplicationBuilder builder) where T2 : DbContext, T
    {
        var options = builder.Configuration.GetSection("DatabaseConnectionString").Get<Options>();

        if (options is null || string.IsNullOrEmpty(options.ConnectionString))
        {
            throw new Exception("'DatabaseConnectionString' not found in configuration");
        }
        
        builder.Services.AddDbContext<T,T2>(x =>
        {
            x.UseSqlServer(options.ConnectionString, y => y.EnableRetryOnFailure());
        });
        
        return builder;
    }
    
    public static WebApplicationBuilder AddGraphQl<TEntity, TSchema>(this WebApplicationBuilder builder) where TSchema : ObjectGraphType<TEntity>
    {
        builder.Services
            .AddGraphQL(builder => builder
                .AddSystemTextJson()
                .AddGraphTypes()
            );

        builder.Services.AddScoped<TSchema>();

        return builder;
    }
    public static WebApplication UseGraphQl(this WebApplication app)
    {
        app.UseGraphQL();
        
        app.UseGraphQLAltair("/graphql/ui");
        
        return app;
    }
}