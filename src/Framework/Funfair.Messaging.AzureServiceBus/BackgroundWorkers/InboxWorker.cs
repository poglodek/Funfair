using Funfair.Messaging.AzureServiceBus.MessageBus;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Funfair.Messaging.AzureServiceBus.BackgroundWorkers;

public class InboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    private OutboxDbContext _dbContext;
    private ILogger<InboxWorker> _logger;
    private IMessageBusOperator _messageBusOperator;
    

    public InboxWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        var serviceProvider = _scopeFactory.CreateAsyncScope().ServiceProvider;
        
        _logger = serviceProvider.GetRequiredService<ILogger<InboxWorker>>();
        _messageBusOperator = serviceProvider.GetRequiredService<IMessageBusOperator>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await periodicTimer.WaitForNextTickAsync(stoppingToken);
            
            _dbContext = serviceProvider.GetRequiredService<OutboxDbContext>();
            
            using var scope = serviceProvider.CreateScope();

            var outboxes = await _dbContext.Outboxes.Where(x => x.SentDate == null && x.ErrorMessage == "" ).ToListAsync(stoppingToken);

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
                    _logger.LogError($"Cannot process outbox message: {outbox.MessageType}",e);
                    outbox.ErrorMessage = e.Message;
                }
                
                _logger.LogDebug($"Processed outbox message: {outbox.MessageType}");
                
            }
            
            await _dbContext.SaveChangesAsync(stoppingToken);
        }
        
        
    }
}