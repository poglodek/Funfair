using Funfair.Auth;
using MediatR;

namespace Users.App.Commands;

public class SignIn : IRequest<JWTokenDto>
{
    public string Email { get; private set; }
    public string Password { get; private set; }

    public SignIn(string email, string password)
    {
        Email = email;
        Password = password;
    }
}