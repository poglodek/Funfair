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
    public static WebApplicationBuilder AddAppByKeyVault(this WebApplicationBuilder builder,string name)
    {
        var options = builder.Configuration.GetSection("KeyVault").Get<Options>();
        
        var client = new SecretClient(new Uri(options.Url),  new DefaultAzureCredential());
        
        var secret =  client.GetSecret($"app-{name}");

        if (!secret.HasValue)
        {
            throw new Exception($"Secret not found for 'app-{name}'");
        }
        
        builder.Configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(secret.Value.Value)));
        
        return builder;
    }
}