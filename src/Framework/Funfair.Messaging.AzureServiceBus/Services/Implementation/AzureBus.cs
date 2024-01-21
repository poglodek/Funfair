using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;
using Funfair.Messaging.AzureServiceBus.Options;
using Microsoft.Extensions.Configuration;

namespace Funfair.Messaging.AzureServiceBus.Services.Implementation;

internal class AzureBus : IAzureBus, IAsyncDisposable
{
    private readonly ConcurrentDictionary<string, ServiceBusSender> _busSenders = new();
    private readonly MessageBusOptions _options;
    private ServiceBusClient _busClient;
    
    public AzureBus(IConfiguration configuration)
    {
        
        _options = configuration.GetSection("AzureMessageBus")
                                .Get<MessageBusOptions>();
        

        if (_options is null || string.IsNullOrEmpty(_options.ConnectionString))
        {
            throw new ArgumentNullException($"{nameof(_options.ConnectionString)} is null");
        }
        
    }

    public ServiceBusProcessor CreateProcessor(string exchange)
    {
        return _busClient.CreateProcessor(exchange);
    }

    public void CreateBus()
    {
        if (!_options.Enabled)
        {
            return;
        }
        
        _busClient = new ServiceBusClient(_options.ConnectionString);
    }

    private ServiceBusSender GetSender(string topic)
    {
        if (_busSenders.TryGetValue(topic, out var sender))
        {
            return sender;
        }

        sender = _busClient.CreateSender(topic);

        _busSenders.TryAdd(topic, sender);

        return sender;
    }

    public ValueTask<ServiceBusMessageBatch> CreateBatchAsync(string topic)
    {
        return GetSender(topic).CreateMessageBatchAsync();
    }

    public async Task SendAsync(string topic,ServiceBusMessageBatch batch)
    {
        await GetSender(topic).SendMessagesAsync(batch);
        
        batch.Dispose();
        
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var sender in _busSenders.Values)
        {
            await sender.DisposeAsync();
        }
        
        await _busClient.DisposeAsync();
    }
}