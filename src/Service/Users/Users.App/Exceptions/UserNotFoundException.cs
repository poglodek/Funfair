namespace Users.App.Exceptions;

public class UserNotFoundException : AppException
{
    public UserNotFoundException(string value) : base($"User with mail {value} not exists")
    {
    }

    public override string ErrorMessage => "user_not_found";
}