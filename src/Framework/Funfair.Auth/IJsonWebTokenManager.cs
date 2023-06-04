using Microsoft.IdentityModel.JsonWebTokens;

namespace Disco.Shared.Auth;

public interface IJsonWebTokenManager
{
    JWTokenDto CreateToken(Guid userId, string email, string role, IDictionary<string,string> claims = null);
}