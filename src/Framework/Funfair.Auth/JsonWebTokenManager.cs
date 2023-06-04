using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Funfair.Auth;

public class JsonWebTokenManager : IJsonWebTokenManager
{
    private readonly AuthOptions _options;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    
    public JsonWebTokenManager(AuthOptions options, AuthExtensions.SecurityKeyCert key)
    {
        _options = options;
        _signingCredentials = new SigningCredentials(key.Key, SecurityAlgorithms.HmacSha256);
    }

    public JWTokenDto CreateToken(int userId, string email, string role, IDictionary<string,string> claims = null)
    {
        if (claims is null)
        {
            claims = new Dictionary<string, string>(0);
        }

        var jwtClaims = new List<Claim>(claims.Count);


        if (claims.Any())
        {
            foreach (var claim in claims)
            {
                jwtClaims.Add(new Claim(claim.Key, claim.Value));
            }
        }
        
        jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        jwtClaims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()));
        jwtClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));

        var expires = DateTime.Now.AddHours(_options.ExpiresInHours);

        var jwt = new JwtSecurityToken(_options.JwtIssuer, 
            _options.JwtIssuer,
            claims: jwtClaims, 
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: _signingCredentials);


        var token = _jwtSecurityTokenHandler.WriteToken(jwt);

        return new JWTokenDto
        {
            JWT = token,
            ExpiresInHours = _options.ExpiresInHours,
            UserId = userId,
            Role = role
        };

    }
}