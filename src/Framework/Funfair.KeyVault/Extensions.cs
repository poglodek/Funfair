using System.Reflection;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Funfair.KeyVault.Token;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;


namespace Funfair.KeyVault;

public static class Extensions
{
    
    public static WebApplicationBuilder AddKeyVault(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.GetSection("KeyVault").Get<KeyVaultOptions>();

        if (!options.Enabled)
        {
            return builder;
        }

        if (options.Keys is null || options.Keys.Length == 0)
        {
            throw new ArgumentNullException("Keys not found in configuration for 'KeyVault'");
        }
        
        var client = new SecretClient(new Uri(options.Url),  new DefaultAzureCredential());
        
        
        foreach (var key in options.Keys)
        {
            var secret =  client.GetSecret(key);

            if (!secret.HasValue)
            {
                throw new Exception($"Secret not found for '{key}' in 'KeyVault'");
            }

            var name = secret.Value.Properties.Name.Replace("-", ":");
            
            builder.Configuration[name] = secret.Value.Value;
        }

        return builder;
    }
    
    
}