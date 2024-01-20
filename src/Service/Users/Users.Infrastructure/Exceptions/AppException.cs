namespace Users.Infrastructure.Exceptions;

public abstract class AppException : Exception
{
    public abstract string ErrorMessage { get; }

    protected AppException(string msg) : base(msg)
    {
    }


}