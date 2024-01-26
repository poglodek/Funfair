using Azure.Messaging.ServiceBus;
using Funfair.Messaging.AzureServiceBus.Options;
using Microsoft.Extensions.Configuration;

namespace Funfair.Messaging.AzureServiceBus.Services.Implementation;

internal class AzureBus : IAzureBus, IAsyncDisposable
{
    private readonly MessageBusOptions _options;
    private ServiceBusClient _busClient;
    
    public AzureBus(IConfiguration configuration)
    {
        
        _options = configuration.GetSection("AzureMessageBus")
                                .Get<MessageBusOptions>();
        

        if (_options is null ||( string.IsNullOrEmpty(_options.ConnectionString) && _options.Enabled))
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
    

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        if (_busClient is not null)
        {
            await _busClient.DisposeAsync();
        }
    }
}