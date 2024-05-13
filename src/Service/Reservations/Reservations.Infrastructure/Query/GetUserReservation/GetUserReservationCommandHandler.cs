using Funfair.Shared.App.Auth;
using MediatR;
using Reservations.Core.Repository;
using Reservations.Infrastructure.Dtos;

namespace Reservations.Infrastructure.Query.GetUserReservation;

public class GetUserReservationCommandHandler(IReservationRepository repository, IUserContextAccessor contextAccessor) : IRequestHandler<GetUserReservationCommand, UserReservationsDto>
{
    public async Task<UserReservationsDto> Handle(GetUserReservationCommand request, CancellationToken cancellationToken)
    {
        var userId = contextAccessor.Get().UserId;
        var reservations = await repository.GetUserReservation(userId, cancellationToken);

        return new UserReservationsDto(reservations
            .Select(c =>
                new UserReservationDto(c.Id, c.UserReservations
                                            .FirstOrDefault(z => z.User.Id == userId)
                                            ?.Id))
            .ToList());
    }
}