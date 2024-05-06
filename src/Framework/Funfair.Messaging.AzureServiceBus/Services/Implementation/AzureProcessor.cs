using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Azure.Messaging.ServiceBus;
using Funfair.Messaging.AzureServiceBus.Models;
using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Funfair.Shared.App.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Funfair.Messaging.AzureServiceBus.Services.Implementation;

internal class AzureProcessor : IAzureProcessor, IDisposable, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAzureBus _bus;
    private readonly ILogger<AzureProcessor> _logger;
    private readonly IReadOnlyCollection<Type> _types;
    private readonly ConcurrentDictionary<string, ServiceBusProcessor> _busProcessors = new();

    public AzureProcessor(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        _serviceProvider = serviceProvider;
        _bus = scope.ServiceProvider.GetRequiredService<IAzureBus>();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<AzureProcessor>>();
        _types = GetTypes();
    }
    
    public async Task StartProcessing()
    {
        var exchanges = _types
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

    private async Task ProcessorOnProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        _logger.LogInformation($"Got new message {arg.Message.Subject} with id - {arg.Message.MessageId}");

        await using var scope = _serviceProvider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var inbox = new MessageInbox
        {
            MessageId = Guid.Parse(arg.Message.MessageId),
            MessageType = arg.Message.Subject,
            Message = arg.Message.Body.ToString()
        };
        
        var key = inbox.MessageType;

        _logger.LogInformation($"Processing an event with name {key}");

        var type = _types.FirstOrDefault(x => x.Name == key);

        if (type is null)
        {
            _logger.LogError($"Cannot find a type with name {key}");
            return;
        }

        var obj = JsonConvert.DeserializeObject(inbox.Message, type);

        if (obj is null)
        {
            _logger.LogError($"Cannot deseralize an object with name {inbox.MessageType}");
            return ;
        }

        await mediator.Publish(obj, arg.CancellationToken);

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
    
    
    private static ReadOnlyCollection<Type> GetTypes()
    {
        return new AssembliesService()
            .ReturnTypes()
            .Where(t => typeof(IIntegrationEvent).IsAssignableFrom(t)
                        && t.GetCustomAttributes(typeof(MessageAttribute), true).Length > 0)
            .ToList()
            .AsReadOnly();
    }

    
}