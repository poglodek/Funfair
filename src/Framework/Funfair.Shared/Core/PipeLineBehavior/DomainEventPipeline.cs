using Funfair.Shared.Core.Repository;
using Funfair.Shared.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Funfair.Shared.Core.PipeLineBehavior;

internal sealed class DomainEventPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IChangeTrack _changeTrack;
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventPipeline<TRequest, TResponse>> _logger;

    public DomainEventPipeline(IChangeTrack changeTrack, IMediator mediator, ILogger<DomainEventPipeline<TRequest, TResponse>> logger)
    {
        _changeTrack = changeTrack;
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var domainEvents = _changeTrack.ChangeTracker.Entries<DomainBase>()
            .Select(x => x.Entity as DomainBase)
            .SelectMany(x => x?.DomainEvents)
            .ToList();
           
        
        if (domainEvents is not null && domainEvents.Any())
        {
            _logger.LogInformation($"Found {domainEvents.Count} domain events.");

            foreach (var @event in domainEvents)
            {
                _logger.LogInformation($"Publishing domain event {@event.GetType().Name}");
                
                await _mediator.Publish(@event, cancellationToken);
                
                _logger.LogInformation("Published domain event.");
            }
        }
        

        return response;
    }
}