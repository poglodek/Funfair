using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.DAL.MsSql;

public static class Extensions
{
    public static WebApplicationBuilder AddMsSql<T>(this WebApplicationBuilder builder) where T : DbContext
    {
        var options = builder.Configuration.GetSection("DatabaseConnectionString").Get<Options>();

        if (options is null || string.IsNullOrEmpty(options.ConnectionString))
        {
            throw new Exception("'DatabaseConnectionString' not found in configuration");
        }
        
        builder.Services.AddDbContext<T>(x =>
        {
            x.UseSqlServer(options.ConnectionString, y => y.EnableRetryOnFailure());
        });
        
        return builder;
    }
    
    public static WebApplicationBuilder AddGraphQl<TSchema>(this WebApplicationBuilder builder) where TSchema : class
    {
        builder.Services.AddControllers();
        builder.Services.AddGraphQLServer().AddQueryType<TSchema>().AddProjections().AddFiltering().AddSorting();
        
        return builder;
    }
    public static WebApplication UseGraphQl(this WebApplication app)
    {
        app.MapControllers();
        app.MapGraphQL("/graphql");
        
        return app;
    }
}