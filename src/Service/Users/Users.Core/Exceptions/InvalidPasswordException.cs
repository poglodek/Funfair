using Funfair.Shared.Exception;

namespace Users.Core.Exceptions;

public class InvalidPasswordException : CoreException
{
    public InvalidPasswordException(string value) : base($"Invalid password - {value}")
    {
    }

    public override string ErrorMessage => "invalid_password";
}