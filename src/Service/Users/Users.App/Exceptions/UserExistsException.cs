using Funfair.Shared.App;

namespace Users.App.Exceptions;

public class UserExistsException(string value) : AppException($"User with email {value} exists")
{
    public override string ErrorMessage => "user_exists";
}