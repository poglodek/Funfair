using MediatR;
using Planes.Infrastructure.Dto;

namespace Planes.Infrastructure.Query;

public record GetPlanesQuery() : IRequest<PlanesDto>;