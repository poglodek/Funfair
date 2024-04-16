namespace Funfair.Auth;

public interface IJsonWebTokenManager
{
    JwtTokenDto CreateToken(Guid userId, string email, string role, IDictionary<string,string> claims = null);
}