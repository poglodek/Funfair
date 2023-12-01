namespace Funfair.Shared.Exception;

public abstract class CoreException : System.Exception
{
    public abstract string ErrorMessage { get; }
    
    protected CoreException(string value) : base(value)
    {
        
    }
}