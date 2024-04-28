using Funfair.Shared.App.Auth;
using Funfair.Shared.App.Auth.UserContext;

namespace Test.Shared;

public class UserContextAccessorTest(Guid userId, string role,Dictionary<string,string> claims) : IUserContextAccessor
{
    public IUserContext Get()
    {
        return new UserContext(userId, role, claims);
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

    public void CheckIfUserHasClaim(string claim)
    {
        Get(claim);
    }
}