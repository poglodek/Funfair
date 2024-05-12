using MediatR;
using Reservations.Core.Repository;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetUserReservation;

public class GetUserReservationCommandHandler(IReservationRepository repository) : IRequestHandler<GetUserReservationCommand, UserReservationsDto>
{
    public async Task<UserReservationsDto> Handle(GetUserReservationCommand request, CancellationToken cancellationToken)
    {
        var reservations = await repository.GetUserReservation(request.UserId, cancellationToken);

        return new UserReservationsDto(reservations
            .Select(c =>
                new UserReservationDto(c.Id, c.UserReservations
                                            .FirstOrDefault(z => z.User.Id == request.UserId)
                                            ?.Id))
            .ToList());
    }
}