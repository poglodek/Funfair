using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;
using Funfair.Messaging.AzureServiceBus.Models;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Funfair.Messaging.AzureServiceBus.Services.Implementation;

internal class AzureProcessor : IAzureProcessor, IDisposable, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAzureBus _bus;
    private readonly IAssembliesService _assembliesService;
    private readonly ILogger<AzureProcessor> _logger;
    private readonly ConcurrentDictionary<string, ServiceBusProcessor> _busProcessors = new();

    public AzureProcessor(IServiceProvider serviceProvider, ILogger<AzureProcessor> logger
        )
    {
        var scope = serviceProvider.CreateScope();
        
        _assembliesService = new AssembliesService();
        _serviceProvider = serviceProvider;
        _bus = scope.ServiceProvider.GetRequiredService<IAzureBus>();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<AzureProcessor>>();
    }
    
    public async Task StartProcessing()
    {
        var exchanges = _assembliesService.ReturnTypes()
            .Select(type => new
            {
                Attribute = (MessageAttribute) Attribute.GetCustomAttribute(type, typeof(MessageAttribute))
            })
            .Where(t => t.Attribute != null)
            .Select(c=> c.Attribute.Exchange);
        
        foreach (var exchange in exchanges)
        {
            var processor = _bus.CreateProcessor(exchange);
            _busProcessors.TryAdd(exchange, processor);
            
            processor.ProcessMessageAsync += ProcessorOnProcessMessageAsync;
            processor.ProcessErrorAsync += ProcessorOnProcessErrorAsync;
            await processor.StartProcessingAsync();
        }
        
    }

    private Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError($"Error on process -> {arg.Exception.Message}, {arg.ErrorSource.ToString()}, {arg.FullyQualifiedNamespace}, {arg.Identifier}");
        
        return Task.CompletedTask;
    }

    private Task ProcessorOnProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        _logger.LogInformation($"Got new message {arg.Message.Subject} with id - {arg.Message.MessageId}");
        
        var outboxContainer = _serviceProvider.CreateAsyncScope().ServiceProvider.GetRequiredService<OutBoxContainer>();
        
        var inbox = new Inbox
        {
            Id = Guid.NewGuid(),
            MessageId = Guid.Parse(arg.Message.MessageId),
            DateReceived = DateTime.Now,
            MessageType = arg.Message.Subject,
            Message = arg.Message.Body.ToString()
        };
        
        return outboxContainer.Container.CreateItemAsync(inbox, new PartitionKey(inbox.MessageId.ToString()));
    }


    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var processor in _busProcessors.Values)
        {
            await  processor.StopProcessingAsync();
            await processor.DisposeAsync();
        }
    }

    
}