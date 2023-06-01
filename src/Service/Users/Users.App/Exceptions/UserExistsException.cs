namespace Users.App.Exceptions;

public class UserExistsException : AppException
{
    public UserExistsException(string value) : base($"User with email {value} exists")
    {
    }

    public override string ErrorMessage => "user_exists";
}