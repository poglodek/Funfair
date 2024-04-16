using System.Security.Claims;
using Funfair.Shared.App.Auth.UserContext;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Funfair.Shared.App.Auth;

internal class UserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    
    public IUserContext Get()
    {
        var claimsIdentity = _httpContextAccessor!.HttpContext!.User.Identity as ClaimsIdentity;

        if (claimsIdentity is null)
        {
            throw new UnauthorizedAccessException();
        }
        
        var claims = claimsIdentity.Claims;

        if (claims.Any())
        {
            throw new UnauthorizedAccessException();
        }
        
        var userId = Guid.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value);
        var role = claims.First(x => x.Type == ClaimTypes.Role).Value;

        return new UserContext.UserContext(userId, role, claims.ToDictionary(x => x.Type, x => x.Value));

    }

    public IUserContext Get(string claim)
    {
        var userContext =  Get();
        
        if(!userContext.GetClaims.ContainsKey(claim))
        {
            throw new UnauthorizedAccessException();
        }
        
        return userContext;
    }
}