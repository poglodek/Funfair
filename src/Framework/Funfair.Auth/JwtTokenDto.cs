namespace Funfair.Auth;

public record JwtTokenDto
{
    public string JWT { get; init; }
    public string Role { get; init; }
    public Guid UserId { get; init; }
    public int ExpiresInHours { get; init; }
}