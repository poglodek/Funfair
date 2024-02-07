namespace Funfair.Shared.App;

public abstract class AppException(string value) : System.Exception(value)
{
    public abstract string ErrorMessage { get; }
}