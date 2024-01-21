using Funfair.Shared.App.Command;
using Funfair.Shared.Core.Repository;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Funfair.Shared.App.PipeLineBehavior;

internal sealed class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly ICosmosUnitOfWork _cosmosUnitOfWork;
    private readonly ILogger<UnitOfWorkPipeline<TRequest, TResponse>> _logger;

    public UnitOfWorkPipeline(ICosmosUnitOfWork cosmosUnitOfWork, ILogger<UnitOfWorkPipeline<TRequest, TResponse>> logger)
    {
        _cosmosUnitOfWork = cosmosUnitOfWork;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        
        TransactionalBatch? batch = null;

        if (request is IRequestTransactionalCommand<TResponse> transactionalRequest)
        {
            _logger.LogDebug("Creating transactional batch...");
            
            batch = _cosmosUnitOfWork.CreateTransactionalBatch();
            transactionalRequest.TransactionalBatch = batch;
            
            _logger.LogDebug("Transactional batch created");
        }

        var response = await next();

        if (batch is not null)
        {
            _logger.LogDebug("Committing transactional batch...");

            try
            {
                await _cosmosUnitOfWork.CommitAsync(batch, cancellationToken);
            
                _logger.LogDebug("Transaction committed");
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error committing transactional batch: {e.Message}");
                
                throw;
            }
        }

        return response;

    }
}