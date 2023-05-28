namespace Users.Core.Exceptions;

public class InvalidNameException : CoreException
{
    public InvalidNameException(string value) : base($"Invalid name - {value}")
    {
    }

    public override string ErrorMessage => "invalid_name";
}