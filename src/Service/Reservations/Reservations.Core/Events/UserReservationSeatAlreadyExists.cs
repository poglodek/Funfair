using Funfair.Shared.Exception;

namespace Reservations.Core.Events;

public class UserReservationSeatAlreadyExists(string value) :CoreException(value)
{
    public override string ErrorMessage => "seat_already_reserved";
}