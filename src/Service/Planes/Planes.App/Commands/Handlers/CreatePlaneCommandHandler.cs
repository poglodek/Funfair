using MediatR;
using Planes.App.Commands.Dto;
using Planes.App.Exceptions;
using Planes.App.Repositories;
using Planes.Core.Builder;

namespace Planes.App.Commands.Handlers;

public class CreatePlaneCommandHandler(IPlaneRepository planeRepository) : IRequestHandler<CreatePlaneCommand, PlaneIdDto>
{
    public async Task<PlaneIdDto> Handle(CreatePlaneCommand request, CancellationToken cancellationToken)
    {
        var existingPlane = await planeRepository.GetByModelAsync(request.Model, cancellationToken);

        if (existingPlane is not null)
        {
            throw new PlaneExistsException(existingPlane.Model);
        }


        var builder = new PlaneBuilder()
            .WithModel(request.Model)
            .WithProductionYear(request.ProductionYear);

        if(request.SeatsList is not null)
        {
            foreach (var seats in request.SeatsList)
            {
                builder.WithSeats(seats.Rows, seats.NumberInRow, seats.SeatClass);
            }
        }

        var plane = builder.Build();
        
        await planeRepository.AddAsync(plane, cancellationToken);

        return new PlaneIdDto(plane.Id);
    }
}