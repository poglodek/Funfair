namespace Funfair.Messaging.EventHubs.Exception;

public class InvalidExchangeException : System.Exception
{
    public InvalidExchangeException(string msg) : base(msg)
    {
        
    }
}