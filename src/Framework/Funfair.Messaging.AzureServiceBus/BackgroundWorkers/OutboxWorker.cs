using Funfair.Messaging.AzureServiceBus.MessageBus;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.Query;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Funfair.Messaging.AzureServiceBus.BackgroundWorkers;

internal class OutboxWorker : BackgroundService
{
    private readonly ILogger<OutboxWorker> _logger;
    private readonly IMessageBusOperator _messageBusOperator;
    private readonly IServiceScopeFactory _scopeFactory;


    public OutboxWorker(ILogger<OutboxWorker> logger, IMessageBusOperator busOperator, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _messageBusOperator = busOperator;
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
            
            var outBoxContainer = scope.ServiceProvider.GetRequiredService<OutBoxContainer>();
            var outboxes = await outboxQuery.GetUnprocessedInboxesAsync(stoppingToken);

            foreach (var outbox in outboxes)
            {
                _logger.LogDebug($"Processing outbox message: {outbox.MessageType}");

                try
                {
                    await _messageBusOperator.Publish(outbox);

                    outbox.SentDate = DateTime.Now;
                }
                catch (System.Exception e)
                {
                    _logger.LogError($"Cannot process outbox message: {outbox.MessageType}", e);
                    outbox.ErrorMessage = e.Message;
                }
                finally
                {
                    outBoxContainer.Container.UpsertItemAsync(outbox, new PartitionKey(outbox.Id.ToString()), cancellationToken: stoppingToken);
                }
                
                _logger.LogDebug($"Processed outbox message: {outbox.MessageType}");
                
            }
            
        }
        
        
    }
}