using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using Funfair.KeyVault.Services;
using Funfair.KeyVault.Token;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funfair.KeyVault;

public static class Extensions
{
    public static IServiceCollection AddKeyVault(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("KeyVault").Get<Options>();
        
        services.AddSingleton(_ => options);

        services.AddSingleton<IKeyVault,Services.KeyVault>(_ =>
        {
            var client = new SecretClient(new Uri(options.Url), new CustomTokenCredential(options.Token));

            return new Services.KeyVault(client);
        });
        
        return services;
    }
}