namespace Funfair.Messaging.AzureServiceBus.Exception;

public class InvalidMessageSize : System.Exception
{
    private InvalidMessageSize(string msg) : base(msg) 
    {
        
    }

    public static void ThrowIfFalse(bool con)
    {
        if (!con)
        {
            throw new InvalidMessageSize($"Invalid size of message");
        }
    }
}