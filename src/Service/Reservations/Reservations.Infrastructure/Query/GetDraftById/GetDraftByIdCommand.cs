using MediatR;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetDraftById;

public record GetDraftByIdCommand(Guid Id) : IRequest<DraftDto>;