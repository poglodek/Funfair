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
}