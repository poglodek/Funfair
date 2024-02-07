using MediatR;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Planes.App.Repositories;
using Planes.Infrastructure.Dto;

namespace Planes.Infrastructure.Query.QueryHandler;

public class GetPlanesQueryHandler(IPlaneRepository planeRepository) : IRequestHandler<GetPlanesQuery, PlanesDto>
{

    public async Task<PlanesDto> Handle(GetPlanesQuery request, CancellationToken cancellationToken)
    {
        var planes = await planeRepository.GetAll(cancellationToken);
        return new PlanesDto(planes.Select(x=> new PlaneInfoDto(x.Id,x.Model)).ToList());
    }
}