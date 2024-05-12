using MediatR;
using Reservations.Core.Repository;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetDraftById;

public class GetDraftByIdCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<GetDraftByIdCommand, DraftDto>
{

    public async Task<DraftDto> Handle(GetDraftByIdCommand request, CancellationToken cancellationToken)
    {
        var draft = await reservationRepository.GetDraftById(request.Id, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(draft, nameof(draft));
        
        
        return new DraftDto(draft.Id, draft.Journey, draft.FlightDate, draft.CreatedBy.Id, draft.Plane.Id);
    }
}