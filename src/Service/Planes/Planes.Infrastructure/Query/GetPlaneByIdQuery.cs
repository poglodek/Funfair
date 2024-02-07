using MediatR;
using Planes.Infrastructure.Dto;

namespace Planes.Infrastructure.Query;

public record GetPlaneByIdQuery(Guid Id) : IRequest<PlaneDto>;