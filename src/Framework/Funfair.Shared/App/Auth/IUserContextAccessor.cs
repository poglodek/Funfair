using Funfair.Shared.App.Auth.UserContext;

namespace Funfair.Shared.App.Auth;

public interface IUserContextAccessor
{
    IUserContext Get();
    IUserContext Get(string claim);
    void CheckIfUserHasClaim(string claim);
}