namespace Funfair.Messaging.AzureServiceBus.Exception;

public class InvalidMessageSize : System.Exception
{
    public InvalidMessageSize(string msg) : base(msg) 
    {
        
    }

    public static void ThrowIfFalse(bool con,int id)
    {
        if (con)
        {
            throw new InvalidMessageSize($"Invalid size of message of id - {id}");
        }
    }
}