namespace Funfair.KeyVault.Services;

public interface IKeyVault 
{
    Task<string> GetSecretAsync(string secretName);
}