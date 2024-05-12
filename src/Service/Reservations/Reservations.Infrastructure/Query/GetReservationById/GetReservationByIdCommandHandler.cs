using MediatR;
using Reservations.Core.Repository;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetReservationById;

public class GetReservationByIdCommandHandler(IReservationRepository reservationRepository) : IRequestHandler<GetReservationByIdCommand,ReservationDto>
{
    public async Task<ReservationDto> Handle(GetReservationByIdCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetById(request.Id, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(reservation, nameof(reservation));
        
        return new ReservationDto(reservation.Id, reservation.Journey, reservation.FlightDate, reservation.CreatedBy.Id, reservation.Plane.Id, 
            reservation.StandardPrice, reservation.UserReservations.Select(x=>new TakenSeatDto(x.Seat.Id.Id)).ToList());
    }
}