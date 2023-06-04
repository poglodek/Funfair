namespace Funfair.Auth;

public interface IJsonWebTokenManager
{
    JWTokenDto CreateToken(int userId, string email, string role, IDictionary<string,string> claims = null);
}