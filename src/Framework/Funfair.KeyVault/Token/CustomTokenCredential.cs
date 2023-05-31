using Azure.Core;

namespace Funfair.KeyVault.Token;

public class CustomTokenCredential : TokenCredential
{
    private readonly string _token;

    public CustomTokenCredential(string token)
    {
        _token = token;
    }
    
    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(new AccessToken(_token, DateTimeOffset.Now.AddHours(12)));
    }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return new AccessToken(_token, DateTimeOffset.Now.AddHours(12));
    }
}