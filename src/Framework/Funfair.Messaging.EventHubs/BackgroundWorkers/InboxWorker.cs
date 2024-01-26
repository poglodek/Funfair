using System.Collections.ObjectModel;
using Funfair.Messaging.EventHubs.Events;
using Funfair.Messaging.EventHubs.Models;
using Funfair.Messaging.EventHubs.OutInBoxPattern;
using Funfair.Messaging.EventHubs.Query;
using Funfair.Messaging.EventHubs.Services.Implementation;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Funfair.Messaging.EventHubs.BackgroundWorkers;

internal class InboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PeriodicTimer _periodicTimer;
    private readonly ILogger<InboxWorker> _logger;
    private IMediator _mediator;
    private IReadOnlyCollection<Type> _types;
    
    public InboxWorker(IServiceScopeFactory scopeFactory, ILogger<InboxWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scopeServiceProvider = _scopeFactory.CreateAsyncScope();
        var serviceProvider = scopeServiceProvider.ServiceProvider;
        
        _mediator = serviceProvider.GetRequiredService<IMediator>();

        _types = GetTypes();

        if (!_types.Any())
        {
            return;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await _periodicTimer.WaitForNextTickAsync(stoppingToken);
            
            var inBoxContainer = serviceProvider.GetRequiredService<InOutBoxContainer>();
            var inboxQuery = serviceProvider.GetRequiredService<IInboxQuery>();
            
            using var scope = serviceProvider.CreateScope();

            var inboxes = await inboxQuery.GetUnprocessedInboxesAsync(stoppingToken);

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
                        _logger.LogError($"Cannot find a type with name {key}");
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
                    _logger.LogError($"Cannot process outbox message: {inbox.MessageType}", e);
                    inbox.ErrorMessage = e.Message;
                }
                finally
                {
                    await inBoxContainer.Container.UpsertItemAsync(inbox, new PartitionKey(inbox.Id.ToString()), cancellationToken: stoppingToken);
                }
                
                _logger.LogDebug($"Processed outbox message: {inbox.MessageType}");
                
            }
            
        }
    }

    private static ReadOnlyCollection<Type> GetTypes()
    {
        return new AssembliesService()
            .ReturnTypes()
            .Where(t => typeof(IIntegrationEvent).IsAssignableFrom(t)
                        && t.GetCustomAttributes(typeof(EventAttribute), true).Length > 0)
            .ToList()
            .AsReadOnly();
    }
}