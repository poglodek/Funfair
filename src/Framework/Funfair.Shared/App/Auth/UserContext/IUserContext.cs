namespace Funfair.Shared.App.Auth.UserContext;

public interface IUserContext
{
    public Guid UserId { get; }
    public string Role { get; }
    Dictionary<string,string> GetClaims { get; }
}