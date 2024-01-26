namespace Funfair.Messaging.EventHubs.Exception;

public class InvalidMessageSize : System.Exception
{
    public InvalidMessageSize(string msg) : base(msg) 
    {
        
    }

    public static void ThrowIfFalse(bool con,Guid id)
    {
        if (!con)
        {
            throw new InvalidMessageSize($"Invalid size of message of id - {id}");
        }
    }
}