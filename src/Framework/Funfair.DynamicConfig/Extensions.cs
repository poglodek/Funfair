using System.Reflection;
using System.Text;
using Funfair.KeyVault.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.DynamicConfig;

public static class Extensions
{
    public static IServiceCollection AddDynamicConfig<T>(this IServiceCollection services, IConfiguration configuration) where T : class
    {
        var key = services.BuildServiceProvider().GetRequiredService<IKeyVault>();

        var name = ReturnAssemblyName();

        var apiConfig =  key.GetSecret($"appsettings-{name}");

        var configBuilder = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(apiConfig)));

        services.Configure<T>(x =>
        { 
            configBuilder.Build().GetSection(typeof(T).Name).Bind(x);
        });
        
        
        return services;
    }

    private static string ReturnAssemblyName()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var assemblyName = entryAssembly.GetName();
        return assemblyName.Name;
    }
}