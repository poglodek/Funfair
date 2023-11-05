using Funfair.Messaging.AzureServiceBus.MessageBus;
using Funfair.Messaging.AzureServiceBus.Models;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.Services;
using Funfair.Messaging.AzureServiceBus.Services.Implementation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Funfair.Messaging.AzureServiceBus.BackgroundWorkers;

public class InboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PeriodicTimer _periodicTimer;
    
    private OutboxDbContext _dbContext;
    private ILogger<InboxWorker> _logger;
    private IMediator _mediator;
    private IEnumerable<Type> _types;
    
    public InboxWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var serviceProvider = _scopeFactory.CreateAsyncScope().ServiceProvider;
        
        _logger = serviceProvider.GetRequiredService<ILogger<InboxWorker>>();
        _mediator = serviceProvider.GetRequiredService<IMediator>();

        _types = new AssembliesService()
            .ReturnTypes()
            .Where(t => typeof(INotification).IsAssignableFrom(t)
                        && t.GetCustomAttributes(typeof(MessageAttribute), true).Length > 0);
        
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await _periodicTimer.WaitForNextTickAsync(stoppingToken);
            
            _dbContext = serviceProvider.GetRequiredService<OutboxDbContext>();
            
            using var scope = serviceProvider.CreateScope();

            var inboxes = await _dbContext.Inboxes.Where(x => x.DateProcessed == null && x.ErrorMessage == "" ).ToListAsync(stoppingToken);

            foreach (var inbox in inboxes)
            {
                _logger.LogDebug($"Processing inbox message: {inbox.MessageType}");
                
                try
                {
                    var key = inbox.MessageType;
                
                    _logger.LogInformation($"Processing an event with name {key}");
                
                    var type = _types.FirstOrDefault(x => x.Name == key);

                    if (type is null)
                    {
                        _logger.LogError( $"Cannot find a type with name {key}");
                        inbox.ErrorMessage = $"Cannot find a type with name {key}";
                        continue;
                    }
                
                    var obj = JsonConvert.DeserializeObject(inbox.Message, type);

                    if (obj is null)
                    {
                        _logger.LogError($"Cannot deseralize an object with name {inbox.Id}");
                        inbox.ErrorMessage = $"Cannot deseralize an object";
                        continue;
                    }
                    
                    await _mediator.Publish(obj, stoppingToken);
                    
                    inbox.DateProcessed = DateTime.Now;
                }
                catch (System.Exception e)
                {
                    _logger.LogError($"Cannot process outbox message: {inbox.MessageType}",e);
                    inbox.ErrorMessage = e.Message;
                }
                
                _logger.LogDebug($"Processed outbox message: {inbox.MessageType}");
                
            }
            
            await _dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}