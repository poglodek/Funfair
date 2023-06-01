namespace Users.App.Exceptions;

public abstract class AppException: Exception
{
    public abstract string ErrorMessage { get; }
    
    protected AppException(string value) : base(value)
    {
        
    }
}