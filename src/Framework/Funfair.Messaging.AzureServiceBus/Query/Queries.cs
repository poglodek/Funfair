namespace Funfair.Messaging.AzureServiceBus.Query;

public class Queries
{
    public const string InBoxQuery = "SELECT * FROM c WHERE c.dateProcessed = null AND c.errorMessage = ''";
    public const string OutBoxQuery = "SELECT * FROM c WHERE c.createdDate = null AND c.errorMessage = ''";
}