namespace Funfair.Messaging.AzureServiceBus.Query;

public class Queries
{
    public const string InBoxQuery = @"SELECT TOP 5 * FROM c WHERE c.dateProcessed = null AND c.errorMessage = '' AND c.type = 'Inbox' ORDER BY c.dateReceived ASC";
    public const string OutBoxQuery = @"SELECT TOP 5 * FROM c WHERE c.createdDate = null AND c.errorMessage = '' AND c.type = 'Outbox' ORDER BY c.createdDate ASC";
}