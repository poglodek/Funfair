using Funfair.Shared.Core.Events;
using MediatR;
using Microsoft.Azure.Cosmos;

namespace Funfair.Shared.App.Command;

public interface IRequestTransactionalCommand<out TResponse> : IRequest<TResponse>
{
    public TransactionalBatch TransactionalBatch { get; set; }
}
