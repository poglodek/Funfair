namespace Funfair.Messaging.AzureServiceBus.Exception;

public class InvalidExchangeException : System.Exception
{
    public InvalidExchangeException(string msg) : base(msg)
    {
        
    }
}