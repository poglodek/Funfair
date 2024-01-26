using Funfair.Messaging.EventHubs.OutInBoxPattern;
using Funfair.Messaging.EventHubs.Query;
using Funfair.Messaging.EventHubs.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Funfair.Messaging.EventHubs.BackgroundWorkers;

internal class OutboxWorker : BackgroundService
{
    private readonly ILogger<OutboxWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;


    public OutboxWorker(ILogger<OutboxWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            
            await periodicTimer.WaitForNextTickAsync(stoppingToken);
            
            var outboxQuery = scope.ServiceProvider.GetRequiredService<IOutboxQuery>();
            var outBoxContainer = scope.ServiceProvider.GetRequiredService<InOutBoxContainer>();
            var eventHubSender = scope.ServiceProvider.GetRequiredService<IEventHubSender>();
            
            
            var outboxes = await outboxQuery.GetUnprocessedInboxesAsync(stoppingToken);

            foreach (var outbox in outboxes)
            {
                _logger.LogDebug($"Processing outbox message: {outbox.MessageType}");

                try
                {
                    await eventHubSender.SendMessagesAsync(outbox,stoppingToken);

                    outbox.SentDate = DateTime.Now;
                }
                catch (System.Exception e)
                {
                    _logger.LogError($"Cannot process outbox message: {outbox.MessageType}", e);
                    outbox.ErrorMessage = e.Message;
                }
                finally
                {
                    await outBoxContainer.Container.UpsertItemAsync(outbox, new PartitionKey(outbox.MessageType), cancellationToken: stoppingToken);
                }
                
                _logger.LogDebug($"Processed outbox message: {outbox.MessageType}");
                
            }
            
        }
        
        
    }
}