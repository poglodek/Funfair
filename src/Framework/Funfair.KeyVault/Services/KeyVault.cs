using Azure.Security.KeyVault.Secrets;

namespace Funfair.KeyVault.Services;

internal sealed class KeyVault : IKeyVault
{
    private readonly SecretClient _client;

    public KeyVault(SecretClient client)
    {
        _client = client;
    }
    
    public async Task<string> GetSecretAsync(string secretName)
    {
        var secret = await  _client.GetSecretAsync(secretName, null, default);
        if (secret.HasValue)
        {
            return secret.Value.Value;
        }

        return string.Empty;
    }
}