using System.Collections.ObjectModel;
using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Funfair.Messaging.EventHubs.Events;
using Funfair.Messaging.EventHubs.Models;
using Funfair.Messaging.EventHubs.Options;
using Funfair.Messaging.EventHubs.OutInBoxPattern;
using Funfair.Messaging.EventHubs.OutInBoxPattern.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Funfair.Messaging.EventHubs.Services.Implementation;

internal sealed class EventHubProcessor : IAsyncDisposable, IEventHubProcessor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventHubProcessor> _logger;
    private readonly IReadOnlyCollection<Type> _types;
    private readonly EventHubsOptions _options;
    private readonly List<EventProcessorClient> clients = new();

    public EventHubProcessor(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        
        _serviceProvider = serviceProvider;
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<EventHubProcessor>>();
        _options = scope.ServiceProvider.GetRequiredService<EventHubsOptions>();
        
        _types = GetTypes();
    }
    
    public void StartProcessing()
    {
        var exchanges = _types
            .Select(type => new
            {
                Attribute = (EventAttribute) Attribute.GetCustomAttribute(type, typeof(EventAttribute))
            })
            .Where(t => t.Attribute != null)
            .Select(c=> c.Attribute.Exchange);
        
        foreach (var exchange in exchanges)
        {
            var storageClient = new BlobContainerClient(_options.BlobConnectionString, exchange);
            var processor = new EventProcessorClient(storageClient, EventHubConsumerClient.DefaultConsumerGroupName, _options.ConnectionString, exchange);
            
            processor.ProcessEventAsync += ProcessorOnProcessMessageAsync;
            processor.ProcessErrorAsync += ProcessorOnProcessErrorAsync;
            
            clients.Add(processor);
        }
        
    }

    private Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError($"Error on process -> {arg.Exception.Message} for {arg.Operation} ");
        
        return Task.CompletedTask;
    }

    private async Task ProcessorOnProcessMessageAsync(ProcessEventArgs arg)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var container = scope.ServiceProvider.GetRequiredService<InOutBoxContainer>().Container;
        
        _logger.LogInformation($"New message received {arg.Data.MessageId}");
        
        var message = Encoding.UTF8.GetString(arg.Data.EventBody.ToArray());
        var type = arg.Data.Properties.TryGetValue(EventConst.MessageType, out var messageType) ? messageType.ToString() : string.Empty;
        
        
        var inbox = new Inbox
        {
            MessageId = Guid.Parse(arg.Data.MessageId),
            Id = Guid.NewGuid(),
            DateReceived = DateTime.UtcNow,
            Message = message,
            MessageType = type
        };
        
        
        await container.UpsertItemAsync(inbox);
        
            
        await arg.UpdateCheckpointAsync(arg.CancellationToken);
        _logger.LogInformation($"Message received {message}");
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var client in clients)
        {
            await client.StartProcessingAsync();
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