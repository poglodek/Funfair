namespace Funfair.Auth;

public sealed class AuthOptions
{
    public string? Url { get; init; }
    public string? Password { get; init; }
    public string? JwtIssuer { get; init; }
    public string? JwtKey { get; init; }
    public int ExpiresInHours { get; init; }
    
}