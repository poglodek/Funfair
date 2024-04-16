namespace Funfair.Shared.App.Auth.UserContext;

public class UserContext(Guid userId, string role, Dictionary<string, string> getClaims)
    : IUserContext
{
    public Guid UserId { get; } = userId;
    public string Role { get; } = role;
    public Dictionary<string, string> GetClaims { get; } = getClaims;
}