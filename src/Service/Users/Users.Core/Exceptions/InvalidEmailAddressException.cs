namespace Users.Core.Exceptions;

public class InvalidEmailAddressException : CoreException
{
    public InvalidEmailAddressException(string value) : base($"Invalid email address - {value}")
    {
    }

    public override string ErrorMessage => "invalid_email_address";
}