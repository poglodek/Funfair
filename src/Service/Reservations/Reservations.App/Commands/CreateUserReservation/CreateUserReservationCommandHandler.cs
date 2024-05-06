using Funfair.Shared.Core;
using Funfair.Shared.Core.Events;
using MediatR;
using Reservations.App.Services;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Core.ValueObjects;

namespace Reservations.App.Commands.CreateUserReservation;

public class CreateUserReservationCommandHandler(IReservationRepository reservationRepository,
    IPlaneService planeService, IClock clock, IEventDispatcher eventDispatcher) : IRequestHandler<CreateUserReservationCommand,Unit>
{
    public async Task<Unit> Handle(CreateUserReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetById(request.ReservationId, cancellationToken);
        var seat = await planeService.GetSeatById(request.SeatId, cancellationToken);
        
        var userReservation = UserReservation.Create(Guid.NewGuid(),new User(request.UserId),seat,reservation.StandardPrice,clock);
        
        
        reservation.AddUserReservation(userReservation,clock);
        
        await eventDispatcher.Publish(userReservation,cancellationToken);
        await eventDispatcher.Publish(reservation,cancellationToken);

        await reservationRepository.Update(reservation, cancellationToken);
        
        
        return Unit.Value;
    }
}