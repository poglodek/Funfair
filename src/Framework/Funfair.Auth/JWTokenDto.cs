namespace Funfair.Auth;

public class JWTokenDto
{
    public string JWT { get; set; }
    public string Role { get; set; }
    public Guid UserId { get; set; }
    public int ExpiresInHours { get; set; }
}