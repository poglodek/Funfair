namespace Users.Core.Exceptions;

public abstract class CoreException : Exception
{
    public abstract string ErrorMessage { get; }
    
    protected CoreException(string value) : base(value)
    {
        
    }
}