namespace Funfair.Shared.Exception;

public abstract class CoreException(string value) : System.Exception(value)
{
    public abstract string ErrorMessage { get; }
}