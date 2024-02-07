using MediatR;
using Planes.App.Repositories;
using Planes.Infrastructure.Dto;
using Planes.Infrastructure.Exceptions;

namespace Planes.Infrastructure.Query.QueryHandler;

public class GetPlaneByIdQueryHandler(IPlaneRepository repository) : IRequestHandler<GetPlaneByIdQuery, PlaneDto>
{
    private readonly IPlaneRepository _repository = repository;

    public async Task<PlaneDto> Handle(GetPlaneByIdQuery request, CancellationToken cancellationToken)
    {
        var plane = await _repository.GetAsync(request.Id, cancellationToken);

        if (plane is null)
        {
            throw new PlaneNotFoundException(request.Id.ToString());
        }

        return new PlaneDto(plane.Id, plane.Model, plane.ProductionYear,
            plane.Seats.Select(x => new SeatsDto(x.RowNumber, x.RowNumber, x.SeatClass)).ToList());
    }
}