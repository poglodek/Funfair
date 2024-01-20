using System.Net;

namespace Users.Infrastructure.Exceptions;

public class CannotAddUserException : AppException
{
    public override string ErrorMessage => "cannot_add_user_to_database";
    
    public CannotAddUserException(string msg) : base(msg)
    {
    }
    public static void ThrowIfStatusCodeNotCreated(HttpStatusCode statusCode)
    {
        if (statusCode != HttpStatusCode.Created)
        {
            throw new CannotAddUserException("Unable to create user");
        }
    }
    
}