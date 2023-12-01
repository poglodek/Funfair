using Funfair.Shared.Core.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Funfair.Shared.App.PipeLineBehavior;

internal sealed class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UnitOfWorkPipeline<TRequest, TResponse>> _logger;

    public UnitOfWorkPipeline(IUnitOfWork unitOfWork, ILogger<UnitOfWorkPipeline<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();
        
        if (typeof(TRequest).Name.EndsWith("Command",StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogInformation("Saving changes...");
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Saved changes.");
        }

        return response;

    }
}