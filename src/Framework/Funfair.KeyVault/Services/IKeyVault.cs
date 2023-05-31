namespace Funfair.KeyVault.Services;

public interface IKeyVault 
{
    Task<string> GetSecretAsync(string secretName);
    string GetSecret(string secretName);
}