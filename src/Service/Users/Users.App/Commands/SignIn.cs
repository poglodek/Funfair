using Funfair.Auth;
using MediatR;

namespace Users.App.Commands;

public class SignIn : IRequest<JWTokenDto>
{
    public string Mail { get; private set; }
    public string Password { get; private set; }

    public SignIn(string mail, string password)
    {
        Mail = mail;
        Password = password;
    }
}