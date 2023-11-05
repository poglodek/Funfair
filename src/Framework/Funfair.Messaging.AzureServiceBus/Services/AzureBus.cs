using System.Collections.Concurrent;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Funfair.Messaging.AzureServiceBus.Options;
using Microsoft.Extensions.Configuration;

namespace Funfair.Messaging.AzureServiceBus.Services;

public class AzureBus : IAzureBus, IAsyncDisposable
{
    private readonly ServiceBusClient _busClient;
    private readonly ConcurrentDictionary<string, ServiceBusSender> _busSenders = new();

    public AzureBus(IConfiguration configuration)
    {
        var options = configuration.GetSection("AzureMessageBus").Get<MessageBusOptions>();

        if (options is null || string.IsNullOrEmpty(options.ConnectionString))
        {
            throw new ArgumentNullException($"{nameof(options.ConnectionString)} is null");
        }

        _busClient = new ServiceBusClient(options.ConnectionString, new DefaultAzureCredential());
   
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
        foreach (var sender in _busSenders.Values)
        { 
            sender.DisposeAsync().GetAwaiter().GetResult();
        }
        
        _busClient.DisposeAsync().GetAwaiter().GetResult();
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