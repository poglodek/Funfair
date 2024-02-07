using Funfair.Shared.App;

namespace Users.App.Exceptions;

public class UserNotFoundException(string value) : AppException($"User with mail {value} not exists")
{
    public override string ErrorMessage => "user_not_found";
}